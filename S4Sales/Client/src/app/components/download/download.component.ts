import { Component, OnInit } from '@angular/core';
import { EcommerceService } from '../../providers/ecommerce.service';

@Component({
  selector: 'download',
  templateUrl: './download.component.html',
  styleUrls: ['./download.component.scss']
})
export class DownloadComponent implements OnInit {
  public Downloads = [];
  constructor(private ec: EcommerceService) { }

  ngOnInit() {
    this.ec.tokens.subscribe( tokens => this.formatDownloadLinks(tokens) );
  }

  download(token: string) { // sends token to be consumed

  }

  formatDownloadLinks(tokens: Array<string>): void {
    tokens.forEach(t => {
      const token = t.split('.'); // splits token string into hsmv_number:token_hash
      const download = {};
      download['hsmv'] = token[0];
      download['token'] = token[1];
      this.Downloads.push(download);
    });
  }
}


// downLoadFile(data: any, type: string) {
//   var blob = new Blob([data], { type: type});
//   var url = window.URL.createObjectURL(blob);
//   var pwa = window.open(url);
//   if (!pwa || pwa.closed || typeof pwa.closed == 'undefined') {
//       alert( 'Please disable your Pop-up blocker and try again.');
//   }
// }


// http://jslim.net/blog/2018/03/13/Angular-4-download-file-from-server-via-http/#disqus_thread
// downloadFile() {
//   return this.http
//     .get('https://jslim.net/path/to/file/download', {
//       responseType: ResponseContentType.Blob,
//       search: // query string if have
//     })
//     .map(res => {
//       return {
//         filename: 'filename.pdf',
//         data: res.blob()
//       };
//     })
//     .subscribe(res => {
//         console.log('start download:',res);
//         var url = window.URL.createObjectURL(res.data);
//         var a = document.createElement('a');
//         document.body.appendChild(a);
//         a.setAttribute('style', 'display: none');
//         a.href = url;
//         a.download = res.filename;
//         a.click();
//         window.URL.revokeObjectURL(url);
//         a.remove(); // remove the element
//       }, error => {
//         console.log('download error:', JSON.stringify(error));
//       }, () => {
//         console.log('Completed file download.')
//       });
// }
