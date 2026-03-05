import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root',
})
export class LanguageService {
  private currentLanguage = new BehaviorSubject<string>(this.getStoredLanguage());
  currentLanguage$ = this.currentLanguage.asObservable();

  private supportedLanguages = ['pt-BR', 'en-US', 'es'];

  constructor(private translateService: TranslateService) {
    // initialize translate service with stored language (or default)
    const lang = this.getStoredLanguage();
    this.translateService.setDefaultLang(lang);

    // defer any use() call until after the injector is finished constructing
    // services; this avoids a circular dependency where HttpClient (used by
    // TranslateService) attempts to instantiate interceptors that depend on
    // LanguageService which is still in its constructor.
    Promise.resolve().then(() => {
      // sync document lang and subject, which also invokes use()
      this.setLanguage(this.currentLanguage.value);
    });
  }

  private getStoredLanguage(): string {
    return localStorage.getItem('language') || 'en-US';
  }

  setLanguage(language: string): void {
    if (this.supportedLanguages.includes(language)) {
      localStorage.setItem('language', language);
      this.currentLanguage.next(language);
      document.documentElement.lang = language;

      // tell ngx-translate to switch.  the HttpLoader will fetch the correct JSON.
      // components using the pipe will update automatically when the
      // TranslateService emits a language change event.
      // use() returns an observable that fires when the file is ready
      this.translateService.use(language).subscribe({
        next: () => console.log(`i18n: switched to language ${language}`),
        error: (err) => console.error('i18n: failed to switch language', language, err),
      });
    }
  }

  getLanguage(): string {
    return this.currentLanguage.value;
  }

  getSupportedLanguages(): string[] {
    return this.supportedLanguages;
  }

  getLanguageLabel(lang: string): string {
    const labels: { [key: string]: string } = {
      'pt-BR': 'Português',
      'en-US': 'English',
      es: 'Español',
    };
    return labels[lang] || lang;
  }
}
