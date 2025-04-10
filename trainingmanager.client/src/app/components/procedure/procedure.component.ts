import { Component, OnInit } from '@angular/core';
import { ProcedureService, ProcedureWithRevision } from '../services/Procedure.Services';

@Component({
  selector: 'app-procedure',
  templateUrl: './procedure.component.html',
  styleUrls: ['./procedure.component.css'],
  standalone: false
})
export class ProcedureComponent implements OnInit {
  procedures: ProcedureWithRevision[] = [];

  selectedProcedure: ProcedureWithRevision | null = null;
  showModal = false;

  revisionHistory: any[] = [];

  constructor(private procedureService: ProcedureService) { }

  // Gets data for the procedures in the procedure tap on the website
  ngOnInit(): void {
    this.procedureService.getProcedureRevisions().subscribe({
      next: (data) => { this.procedures = data;}
      
    });
  }
  
  openRevisionHistory(procedure: ProcedureWithRevision): void {
    this.selectedProcedure = procedure;
    this.showModal = true;

    this.procedureService.getAllRevisionsForProcedure(procedure.procedureName).subscribe({
      next: (data) => {
        this.revisionHistory = data;
      },
      error: (err) => {
        console.error('❌ Failed to load revision history', err);
      }
    });
  }

  //Closes the modal view and removes the data
  closeModal(): void {
    this.showModal = false;
    this.revisionHistory = [];
    this.selectedProcedure = null;
  }
}


