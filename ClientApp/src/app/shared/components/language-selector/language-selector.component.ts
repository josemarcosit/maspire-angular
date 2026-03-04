import { Component, OnInit, HostListener, ElementRef } from '@angular/core';
import { LanguageService } from 'src/app/services/language.service';

@Component({
  selector: 'app-language-selector',
  template: `
    <div class="language-selector dropdown">
      <button
        class="btn btn-sm btn-outline-secondary dropdown-toggle"
        type="button"
        id="languageDropdown"
        [attr.aria-expanded]="isDropdownOpen"
        (click)="toggleDropdown()"
      >
        <i class="fa fa-globe"></i>
        <img
          *ngIf="getFlagImg(getLanguage())"
          [src]="getFlagImg(getLanguage())"
          alt=""
          width="20"
          height="15"
          class="me-1"
        />
        {{ currentLanguageLabel }}
      </button>
      <ul
        class="dropdown-menu"
        [attr.aria-labelledby]="'languageDropdown'"
        [class.show]="isDropdownOpen"
      >
        <li *ngFor="let lang of supportedLanguages">
          <a
            class="dropdown-item"
            [class.active]="isCurrentLanguage(lang)"
            (click)="selectLanguage(lang)"
          >
            <img
              *ngIf="getFlagImg(lang)"
              [src]="getFlagImg(lang)"
              alt=""
              width="20"
              height="15"
              class="me-1"
            />
            {{ getLanguageLabel(lang) }}
          </a>
        </li>
      </ul>
    </div>
  `,
  styles: [
    `
      .language-selector {
        position: relative;
        display: inline-block;
      }
      .btn {
        min-width: 120px;
      }
      .dropdown-menu {
        min-width: 150px;
        display: none;
      }
      .dropdown-menu.show {
        display: block;
      }
      .dropdown-item {
        cursor: pointer;
      }
      .flag {
        margin-right: 8px;
      }
      .dropdown-item.active {
        background-color: #0d6efd;
        color: white;
      }
    `,
  ],
})
export class LanguageSelectorComponent implements OnInit {
  supportedLanguages: string[] = [];
  currentLanguageLabel = '';
  isDropdownOpen = false;

  constructor(
    private languageService: LanguageService,
    private elementRef: ElementRef,
  ) {}

  ngOnInit(): void {
    this.supportedLanguages = this.languageService.getSupportedLanguages();
    this.currentLanguageLabel = this.languageService.getLanguageLabel(
      this.languageService.getLanguage(),
    );

    // Atualizar label quando idioma muda
    this.languageService.currentLanguage$.subscribe((lang: string) => {
      this.currentLanguageLabel = this.languageService.getLanguageLabel(lang);
      this.isDropdownOpen = false; // Fechar o dropdown ao alterar idioma
    });
  }

  selectLanguage(language: string): void {
    const current = this.languageService.getLanguage();
    if (current !== language) {
      this.languageService.setLanguage(language);
      // reload entire app so that server data written in the new culture
      // and any static strings bound via translate pipe are refreshed.
      window.location.reload();
    }
  }

  toggleDropdown(): void {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  isCurrentLanguage(language: string): boolean {
    return this.languageService.getLanguage() === language;
  }

  getLanguageLabel(language: string): string {
    return this.languageService.getLanguageLabel(language);
  }

  getLanguage(): string {
    return this.languageService.getLanguage();
  }

  getFlagImg(language: string): string {
    const map: { [key: string]: string } = {
      'pt-BR': 'https://flagcdn.com/24x18/br.png',
      'en-US': 'https://flagcdn.com/24x18/us.png',
      es: 'https://flagcdn.com/24x18/es.png',
    };
    return map[language] || '';
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.isDropdownOpen = false;
    }
  }
}
