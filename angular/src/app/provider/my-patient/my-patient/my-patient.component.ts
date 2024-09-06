import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { debounceTime, distinctUntilChanged, fromEvent, map, switchMap, tap } from 'rxjs';
import { PagedResultDto } from '@abp/ng.core';
import { GetPatientInput, PatientProviderDto, PatientService } from '@proxy/patients';
import { NotificationListenerService } from 'src/app/services/signalR/notification-listener.service';
@Component({
  selector: 'app-my-patient',
  templateUrl: './my-patient.component.html',
  styleUrl: './my-patient.component.scss'
})
export class MyPatientComponent {
  @ViewChild('search') search !: ElementRef ;
  title = 'type a head search';
  patientFilter = {} as GetPatientInput;
  patients = { items: [], totalCount: 0 } as PagedResultDto<PatientProviderDto>;
  state = false ;

  isModalOpen =false;

  constructor(private service : PatientService ,private NotificationListener : NotificationListenerService){}

  ngOnInit() {
    this.getAllMyPatient();
    //recive notification:
    this.reviceNewPatient();
  }

  getAllMyPatient(){
    this.service.getAllPatientsOfProvider(this.patientFilter).subscribe({
      next : (data:any) => {this.patients = data;}
    });

  }

  ngAfterViewInit(): void {
    fromEvent(this.search.nativeElement,'keyup').pipe(
      debounceTime(100),
      map((evt:any)=>evt.target.value),
      tap(()=>this.state = true) ,
      distinctUntilChanged(),
      switchMap((value:string)=>{
        if (value.length == 0) {
         this.state =false ;
         this.patients = { items: [], totalCount: 0 };
         this.getAllMyPatient();
         return [];
        }
        return this.service.getAllPatientsOfProvider(this.patientFilter);
      }),
      tap(()=>this.state =false)
    ).subscribe((data:any)=> this.patients = data)
  }

  showDetailsOfPatient(rowId){
  }

  reviceNewPatient(){
    this.NotificationListener.reciveNewPatientListener.subscribe((data) => {
      this.getAllMyPatient();
    })
  }
}
