import { Component, EventEmitter, Input, Output } from '@angular/core';
import { any } from 'underscore';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent {
  currentPage = 0;
  pages:number[]= [];
  page = 1;
  @Input() pageSize?: number = 1;
  @Input() totalItems?: number = 10;
  @Output() pageChanged = new EventEmitter<number>();

  ngOnChanges(){
    this.currentPage = 1;
    let a = this.totalItems == undefined ? 1 : this.totalItems;
    let b = this.pageSize == undefined ? 1 : this.pageSize;

	var pagesCount = Math.ceil(a / b);
	for (var i = 1; i <= pagesCount; i++)
		this.pages.push(i);
    
	console.log(this);
	}

	changePage(page: any){
		this.currentPage = page;
		this.pageChanged.emit(page);
	}

	previous(){
		if (this.currentPage == 1)
			return;

		this.currentPage--;
		this.pageChanged.emit(this.currentPage);
	}

	next(){
		if (this.currentPage == this.pages.length)
			return;

		this.currentPage++;
    console.log("next", this);
		this.pageChanged.emit(this.currentPage);
	}
}
