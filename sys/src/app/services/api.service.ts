import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { ApiResponse } from '../core/models/responses/api';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  constructor(
    private _http: HttpClient,
    private _router: Router
  ) { }

  public async Get<T>(
    endpoint: string,
    params?: { [key: string]: any } // Parámetros dinámicos
  ): Promise<ApiResponse<T>> {
    try {
      let httpParams = new HttpParams();
      if (params) {
        for (const key of Object.keys(params)) {
          if (params[key] !== null && params[key] !== undefined) {
            httpParams = httpParams.set(key, params[key]);
          }
        }
      }

      const response = await firstValueFrom(
        this._http.get<ApiResponse<T>>(`/api/${endpoint}`, {
          withCredentials: true,
          params: httpParams,
        })
      );
      return response;
    } catch (error) {
      return this.HandleError(error);
    }
  }

  public async Post<T>(
    endpoint: string,
    body: any
  ): Promise<ApiResponse<T>> {
    try {
      return await firstValueFrom(this._http.post<ApiResponse<T>>(`/api/${endpoint}`, body, { withCredentials: true }));
    } catch (error) {
      return this.HandleError(error);
    }
  }

  public async Put<T>(
    endpoint: string,
    body: any
  ): Promise<ApiResponse<T>> {
    try {
      return await firstValueFrom(this._http.put<ApiResponse<T>>(`/api/${endpoint}`, body, { withCredentials: true }));
    } catch (error) {
      return this.HandleError(error);
    }
  }

  public async Patch<T>(
    endpoint: string,
    body: any
  ): Promise<ApiResponse<T>> {
    try {
      return await firstValueFrom(this._http.patch<ApiResponse<T>>(`/api/${endpoint}`, body, { withCredentials: true }));
    } catch (error) {
      return this.HandleError(error);
    }
  }

  public async Delete<T>(
    endpoint: string
  ): Promise<ApiResponse<T>> {
    try {
      return await firstValueFrom(this._http.delete<ApiResponse<T>>(`/api/${endpoint}`, { withCredentials: true }));
    } catch (error) {
      return this.HandleError(error);
    }
  }

  private HandleError<T>(
    error: any
  ): ApiResponse<T> {
    const message = (() => {
      switch (true) {
        case (error instanceof HttpErrorResponse && !navigator.onLine):
          return 'No hay conexión a internet :C';
        case (error.status === 404 || error.status === 409 || error.status === 400):
          return `${error.error.message}`;
        case (error.status === 401):
          if (error.error && typeof error.error === 'object' && 'success' in error.error && 'message' in error.error) {
            return error.error.message;
          } else {
            this._router.navigateByUrl('/login');
            return 'La sesión expiró';
          }
        case (error instanceof HttpErrorResponse && navigator.onLine):
          return `Error de conexión: (${error.status}) ${error.statusText}`;
        default:
          return error.message ? error.message : error.ToString();
      }
    })();

    return {
      success: false,
      message: message
    };
  }
}
