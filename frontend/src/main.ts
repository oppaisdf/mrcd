import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { RootPage } from './app/core/layout/root.page/root.page';

bootstrapApplication(RootPage, appConfig)
  .catch((err) => console.error(err));
