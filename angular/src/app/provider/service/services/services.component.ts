import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { debounceTime, distinctUntilChanged, finalize, fromEvent, map, switchMap, tap } from 'rxjs';
import { PagedResultDto } from '@abp/ng.core';
import { GetPatientInput, PatientProviderDto, PatientService } from '@proxy/patients';
import { CreateServiceDto, GetServieInput, ServiceDto, ServiceService } from '@proxy/services';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-services',
  templateUrl: './services.component.html',
  styleUrl: './services.component.scss'
})
export class ServicesComponent implements OnInit,AfterViewInit {
  @ViewChild('search') search !: ElementRef ;
  title = 'type a head search';
  serviceFilter = {} as GetServieInput;
  services = { items: [], totalCount: 0 } as PagedResultDto<ServiceDto>;
  state = false ;

  isModalOpen =false;
  isModalOpenForShowImage = false;
  form:FormGroup;
  readonly imageMaxSize = 512000;
  imageError : string;
  selected?: any;


  orginalImage;
  constructor(private service : ServiceService,private fb:FormBuilder,

  ){}

  ngOnInit() {
    this.getAllServices();
  }

  getAllServices(){
    this.service.getListAyncByInput(this.serviceFilter).subscribe({
      next : (data:any) => {this.services = data;console.log(data);
      }
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
         this.services = { items: [], totalCount: 0 };
         this.getAllServices();
         return [];
        }
        return this.service.getListAyncByInput(this.serviceFilter);
      }),
      tap(()=>this.state =false)
    ).subscribe((data:any)=> this.services = data)
  }

  showDetailsOfPatient(rowId){

  }

  openServiceModal(id:string = null){
    if (id) {
      this.getServiceToEdit(id);
    }
    else{
      this.selected = null;
      this.showForm();
    }
  }

  showForm(){
    this.buildForm();
    this.isModalOpen = true;
  }

  buildForm(){
    const {
      title,
      arTitle,
      isActive,
      blop,
      fileType,
      fileName,
      fileSize,
      isIconUpdated
    } = this.selected || {};

    this.form = this.fb.group({
      title : [title ?? '' ,Validators.required],
      arTitle : [arTitle ?? null ,Validators.required],
      isActive : [isActive ?? false, Validators.required],
      blop : [blop ?? null , Validators.required],
      fileType : [fileType ?? 'jpeg',Validators.required],
      fileName : [fileName ?? "" ,Validators.required],
      fileSize : [fileSize ?? 0],
      isIconUpdated :[isIconUpdated ?? false]
    });
  }

  get blop(){
    return this.form.get('blop') as FormControl;
  }
  get fileType(){
    return this.form.get('fileType') as FormControl;
  }
  get fileName(){
    return this.form.get('fileName') as FormControl;
  }
  get fileSize(){
    return this.form.get('fileSize') as FormControl;
  }
  get isIconUpdated(){
    return this.form.get('isIconUpdated') as FormControl;
  }

  save(){
    console.log(this.form.value);
    if (this.form.invalid || this.blop.valid === null || this.blop.value === undefined ) return;
    const request = this.selected
    ? this.service.update(this.selected.id,this.form.value) 
    : this.service.createService(this.form.value);

    request
    .pipe(
      finalize(() => this.isModalOpen = false),
      tap(() => {this.form.reset();}),
    ).subscribe(data => this.getAllServices()) 
  }

  fileChangeListener(fileInput:any){
    console.log(fileInput);
    
    //check if a file has been added
    if (fileInput.target.files && fileInput.target.files[0]) {
      let file = fileInput.target.files[0];
      let fileType = file.type;
      if (fileType !== "image/png" && fileType !== "image/jpeg" && fileType !== "image/jpg") {
        this.imageError = 'UploadImageAcceptedTypesError';
        this.fileType.setValue('jpeg');
        this.blop.setValue('null');
        this.fileSize.setValue(0);
        this.fileName.setValue('');
        return false;
      }
      if (file.size > this.imageMaxSize) {
        this.fileType.setValue('jpeg');
        this.blop.setValue('null');
        this.fileSize.setValue(0);
        this.fileName.setValue('');
        return false;
      }
      const attachmentType = file.name.toLowerCase().substring(file.name.toLowerCase().lastIndexOf('.') + 1);
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => {
        this.fileType.setValue(attachmentType);
        this.blop.setValue(reader.result.toString().replace('data:','').replace(/^.+,/,''));
        this.fileSize.setValue(file.size);
        this.fileName.setValue(file.name.toLowerCase());
        this.isIconUpdated.setValue(true);
      }
    }
  }

  getServiceToEdit(id:any){
    this.service.getService(id).subscribe((data:any) => {
      this.selected = data;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  openImage(image){
    this.isModalOpenForShowImage = true;
    this.orginalImage = image;
  }

  downloadImage(){
    
  }
}
