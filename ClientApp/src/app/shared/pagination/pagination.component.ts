import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css'],
})
export class PaginationComponent {
  currentPage = 0;
  pages: number[] = [];
  page = 1;
  @Input() pageSize?: number = 1;
  @Input() totalItems?: number = 10;
  @Output() pageChanged = new EventEmitter<number>();

  ngOnChanges() {
    this.currentPage = 1;
    const a = this.totalItems === undefined ? 1 : this.totalItems;
    const b = this.pageSize === undefined ? 1 : this.pageSize;

    const pagesCount = Math.ceil(a / b);
    for (let i = 1; i <= pagesCount; i++) {
      this.pages.push(i);
    }
  }

  changePage(page: any) {
    this.currentPage = page;
    this.pageChanged.emit(page);
  }

  previous() {
    if (this.currentPage === 1) {
      return;
    }

    this.currentPage--;
    this.pageChanged.emit(this.currentPage);
  }

  next() {
    if (this.currentPage === this.pages.length) {
      return;
    }

    this.currentPage++;
    this.pageChanged.emit(this.currentPage);
  }
}
