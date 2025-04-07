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

  constructor(private procedureService: ProcedureService) { }

  ngOnInit(): void {
    this.procedureService.getProcedureRevisions().subscribe({
      next: (data) => { console.log('Loaded procedures:', data),  this.procedures = data;}
      
    });
  }

}
