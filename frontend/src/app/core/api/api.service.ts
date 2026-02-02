import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { firstValueFrom, Observable } from 'rxjs';
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

    private async call<T>(obs$: Observable<T>) {
        try {
            const data = await firstValueFrom(obs$);
            return {
                isSuccess: true,
                data
            };
        } catch (error) {
            return {
                isSuccess: false,
                message: parseAPIError(error)
            };
        }
    }

    getAsync<T>(
        endpoint: string,
        params?: Record<string, any>
    ): Promise<ApiResponse<T>> {
        const httpParams = this.toParams(params);
        return this.call(this.http.get<T>(`${API_BASE_URL}${endpoint}`, { params: httpParams }));
    }

    postAsync<TIn, TOut>(
        endpoint: string,
        body: TIn
    ): Promise<ApiResponse<TOut>> {
        return this.call(this.http.post<TOut>(`${API_BASE_URL}${endpoint}`, body));
    }

    delAsync<T>(
        endpoint: string
    ): Promise<ApiResponse<T>> {
        return this.call(this.http.delete<T>(`${API_BASE_URL}${endpoint}`));
    }

    patchAsync<TIn, TOut>(
        endpoint: string,
        body: TIn
    ): Promise<ApiResponse<TOut>> {
        return this.call(this.http.patch<TOut>(`${API_BASE_URL}${endpoint}`, body));
    }
}