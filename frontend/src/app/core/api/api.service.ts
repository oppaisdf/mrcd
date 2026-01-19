import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { parseAPIError } from './api.error.parser';

export const API_BASE_URL = '/api/v2';

@Injectable({
    providedIn: 'root',
})
export class ApiService {
    private http = inject(HttpClient);

    private toParams(
        params?: Record<string, any>
    ) {
        if (!params) return undefined;
        let hp = new HttpParams();
        Object.entries(params)
            .filter(([, v]) => v !== undefined && v !== null)
            .forEach(([k, v]) => { hp = hp.set(k, String(v)); });
        return hp;
    }

    async getAsync<T>(
        endpoint: string,
        params?: Record<string, any>
    ) {
        try {
            const httpParams = this.toParams(params);
            const data = await firstValueFrom(this.http.get<T>(`${API_BASE_URL}${endpoint}`, { params: httpParams }));
            return { success: true, data, status: 200 };
        } catch (err) {
            const { status, message } = parseAPIError(err);
            return { success: false, message, status };
        }
    }

    async postAsync<TIn, TOut>(
        endpoint: string,
        body: TIn
    ) {
        try {
            const data = await firstValueFrom(this.http.post<TOut>(`${API_BASE_URL}${endpoint}`, body));
            return { success: true, data, status: 200 };
        } catch (err) {
            const { status, message } = parseAPIError(err);
            return { success: false, message, status };
        }
    }

    async delAsync<TOut>(
        endpoint: string
    ) {
        try {
            const data = await firstValueFrom(this.http.delete<TOut>(`${API_BASE_URL}${endpoint}`));
            return { success: true, data, status: 200 };
        } catch (err) {
            const { status, message } = parseAPIError(err);
            return { success: false, message, status };
        }
    }

    async patchAsync<TIn, TOut>(
        endpoint: string,
        body: TIn
    ) {
        try {
            const data = await firstValueFrom(this.http.patch<TOut>(`${API_BASE_URL}${endpoint}`, body));
            return { success: true, data, status: 200 };
        } catch (err) {
            const { status, message } = parseAPIError(err);
            return { success: false, message, status };
        }
    }
}
