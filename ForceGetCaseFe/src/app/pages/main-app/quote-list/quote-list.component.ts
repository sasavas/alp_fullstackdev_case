import { Component, OnInit } from '@angular/core';
import { QuoteService, QuoteDto } from '../../../services/quote.service';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzMessageService } from 'ng-zorro-antd/message';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-quote-list',
  standalone: true,
  templateUrl: './quote-list.component.html',
  styleUrls: ['./quote-list.component.scss'],
  imports: [CommonModule, NzTableModule]
})
export class QuoteListComponent implements OnInit {
  quotes: QuoteDto[] = [];
  loading = false;

  constructor(private quoteService: QuoteService, private message: NzMessageService) {}

  ngOnInit(): void {
    this.fetchQuotes();
  }

  fetchQuotes(): void {
    this.loading = true;
    this.quoteService.getAllQuotes().subscribe({
      next: (quotes) => {
        this.quotes = quotes;
        this.loading = false;
      },
      error: (error) => {
        this.message.error('Failed to load quotes: ' + error.message);
        this.loading = false;
      }
    });
  }
}
