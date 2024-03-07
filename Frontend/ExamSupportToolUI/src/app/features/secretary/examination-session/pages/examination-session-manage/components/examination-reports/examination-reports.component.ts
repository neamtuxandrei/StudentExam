import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { environment } from '../../../../../../../../environments/environment';
@Component({
  selector: 'app-examination-reports',
  templateUrl: './examination-reports.component.html',
  styleUrls: ['./examination-reports.component.css']
})
export class ExaminationReportsComponent {

  public examinationSessionId: string | null | undefined = '';
  public environmentData: any = environment;
  constructor(private activatedRoute: ActivatedRoute)
  {
    
  }
  ngOnInit()
  {
    this.examinationSessionId = this.activatedRoute.snapshot.parent?.paramMap.get('id');
  }

}
