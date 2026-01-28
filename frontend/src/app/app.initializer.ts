import { SessionStore } from "./core/stores/session.store";

export function initSessionFactory(
    store: SessionStore
) {
    return () => {
        store.loadFromStorage();
    };
}