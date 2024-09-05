import { Component, Input } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';

@Component({
  selector: 'app-thumbnail',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './thumbnail.component.html',
  styleUrl: './thumbnail.component.scss'
})
export class ThumbnailComponent {
 @Input() imageUrl : string;

 decodeBase64(base64String : string) : string{
  return 'data:image/jpeg;base64,' + base64String; // Adjust the MIME type based on image format
 }
}
