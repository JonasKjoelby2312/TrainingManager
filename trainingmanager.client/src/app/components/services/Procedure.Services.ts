import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface ProcedureWithRevision {
  procedureName: string;
  revisionNumber: number;
  isActive: boolean;
  historyText: string;
}

@Injectable({ providedIn: 'root' })
export class ProcedureService {
  //private apiUrl = 'https://localhost:7139/api/ProcedureOverview';
  
 private apiUrl = 'https://localhost:7227/api/ProcedureOverview'; 

  constructor(private http: HttpClient) { }

  getProcedureRevisions(): Observable<ProcedureWithRevision[]> {
    return this.http.get<ProcedureWithRevision[]>(this.apiUrl);
  }

  //getAllRevisionsForProcedure(procedureName: string): Observable<any[]> {
  //  return this.http.get<any[]>(`${this.apiUrl}/revisions/${procedureName}`);
  //}

  getAllRevisionsForProcedure(procedureName: string): Observable<ProcedureWithRevision[]> {
    return this.http.get<ProcedureWithRevision[]>(`${this.apiUrl}/revisions/${procedureName}`
    );
  }

}
