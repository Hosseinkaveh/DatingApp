import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs/operators";
import { PageInationResult } from "../_models/PageInation";

export function getPageinationResult<T>(url, params, http: HttpClient) {
    const pageInatedResult: PageInationResult<T> = new PageInationResult<T>();

    return http.get<T>(url, { observe: 'response', params }).pipe(
        map(response => {
            pageInatedResult.result = response.body;
            if (response.headers.get('pageination') !== null) {

                pageInatedResult.pageination = JSON.parse(response.headers.get('pageination'));
            }
            return pageInatedResult;

        })
    );
}
export function setParams(PageNumber: number, PageSize: number) {
    let params = new HttpParams();
    params = params.append('PageNumber', PageNumber.toString());
    params = params.append('PageSize', PageSize.toString());
    return params;
}

