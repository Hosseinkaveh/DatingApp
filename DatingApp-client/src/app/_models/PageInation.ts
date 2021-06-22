export interface Pageination{
    itemPerPage:number;
    totalItems:number;
    totalPage:number;
    currentPge:number;
}
export class PageInationResult<T>{
    result:T;
    pageination:Pageination;
}