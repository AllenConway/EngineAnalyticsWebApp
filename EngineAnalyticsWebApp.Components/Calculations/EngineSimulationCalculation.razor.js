// Main-thread bridge between the Blazor component and the .NET Web Worker.
// Creates the worker, waits for its .NET runtime to finish booting, and transports messages.
const pendingRequests = new Map();
let pendingRequestId = 0;
let dotnetWorker;
let readyPromise;
let rejectReady;

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

    const worker = new Worker(new URL('../engineSimulationWorker.js', import.meta.url), { type: 'module' });
    dotnetWorker = worker;
    readyPromise = new Promise((resolve, reject) => {
        rejectReady = reject;

        worker.addEventListener('message', e => {
            if (dotnetWorker !== worker) {
                return;
            }

            switch (e.data.command) {
                case 'ready':
                    resolve();
                    rejectReady = undefined;
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

        worker.addEventListener('error', e => {
            if (dotnetWorker !== worker) {
                return;
            }

            const error = new Error(e.message || 'Engine simulation worker failed to load');
            reject(error);
            rejectPending(error);
        });
    });

    const workerReady = readyPromise;
    workerReady.catch(() => {
        if (dotnetWorker === worker) {
            disposeWorkerInstance(worker);
        }
    });
    return readyPromise;
}

export async function runSimulation(sweeps) {
    const initialization = initializeWorker();
    const worker = dotnetWorker;
    await initialization;
    if (dotnetWorker !== worker) {
        throw new Error('Engine simulation worker was terminated');
    }

    pendingRequestId++;
    const requestId = pendingRequestId;
    const promise = new Promise((resolve, reject) => {
        pendingRequests.set(requestId, { resolve, reject });
    });
    worker.postMessage({ command: 'runSimulation', sweeps, requestId });
    return promise;
}

function disposeWorkerInstance(worker) {
    if (dotnetWorker !== worker) {
        return;
    }

    const initializationReject = rejectReady;
    dotnetWorker = undefined;
    readyPromise = undefined;
    rejectReady = undefined;
    worker.terminate();
    initializationReject?.(new Error('Engine simulation worker was terminated'));
    rejectPending(new Error('Engine simulation worker was terminated'));
}

export function disposeWorker() {
    if (dotnetWorker) {
        disposeWorkerInstance(dotnetWorker);
    } else {
        rejectPending(new Error('Engine simulation worker was terminated'));
    }
}
