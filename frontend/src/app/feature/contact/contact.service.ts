import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { catchError, map } from 'rxjs/operators';

import { throwError } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
    constructor(private http: HttpClient) {}

    getAll() {
        return this.http.get<any[]>(environment.apiEndpoint + '/v1/contact').pipe(
            map((res: any) => res.data),
            catchError(this.handleErrorObservable)
        );
    }

    getById(_id: string) {
        return this.http.get(environment.apiEndpoint + '/v1/contact/' + _id).pipe(
            map((res: any) => res.data),
            catchError(this.handleErrorObservable)
        );
    }

    create(contact: any) {
        return this.http
            .post(environment.apiEndpoint + '/v1/contact', contact)
            .pipe(
                map((res: any) => res.data),
                catchError(this.handleErrorObservable)
            );
    }

    update(contact: any) {
        return this.http
            .put(environment.apiEndpoint + '/v1/contact/' + contact._id, contact)
            .pipe(
                map((res: any) => res.data),
                catchError(this.handleErrorObservable)
            );
    }

    delete(_id: string) {
        return this.http
            .delete(environment.apiEndpoint + '/v1/contact/' + _id)
            .pipe(
                map((res: any) => res.data),
                catchError(this.handleErrorObservable)
            );
    }
    private handleErrorObservable(error: HttpErrorResponse) {
        return throwError(error);
    }
}
