// Main-thread bridge between the Blazor component and the .NET Web Worker.
// Creates the worker, waits for its .NET runtime to finish booting, and transports messages.
const pendingRequests = new Map();
let pendingRequestId = 0;
let dotnetWorker;
let readyPromise;

function rejectPending(error) {
    for (const request of pendingRequests.values()) {
        request.reject(error);
    }
    pendingRequests.clear();
}

export function initializeWorker() {
    if (readyPromise) {
        return readyPromise;
    }

    readyPromise = new Promise((resolve, reject) => {
        dotnetWorker = new Worker(new URL('../engineSimulationWorker.js', import.meta.url), { type: 'module' });

        dotnetWorker.addEventListener('message', e => {
            switch (e.data.command) {
                case 'ready':
                    resolve();
                    break;
                case 'startupError':
                    reject(new Error(e.data.error));
                    break;
                case 'response': {
                    const request = pendingRequests.get(e.data.requestId);
                    pendingRequests.delete(e.data.requestId);
                    if (!request) {
                        break;
                    }
                    if (e.data.error) {
                        request.reject(new Error(e.data.error));
                    } else {
                        request.resolve(e.data.result);
                    }
                    break;
                }
                default:
                    console.log('Engine simulation worker said:', e.data);
            }
        });

        dotnetWorker.addEventListener('error', e => {
            const error = new Error(e.message || 'Engine simulation worker failed to load');
            reject(error);
            rejectPending(error);
        });
    });

    readyPromise.catch(() => disposeWorker());
    return readyPromise;
}

export async function runSimulation(sweeps) {
    await initializeWorker();
    pendingRequestId++;
    const requestId = pendingRequestId;
    const promise = new Promise((resolve, reject) => {
        pendingRequests.set(requestId, { resolve, reject });
    });
    dotnetWorker.postMessage({ command: 'runSimulation', sweeps, requestId });
    return promise;
}

export function disposeWorker() {
    if (dotnetWorker) {
        dotnetWorker.terminate();
    }
    rejectPending(new Error('Engine simulation worker was terminated'));
    dotnetWorker = undefined;
    readyPromise = undefined;
}
