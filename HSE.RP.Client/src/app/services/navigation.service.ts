import { Injectable } from "@angular/core";
import { ActivatedRoute, Params, Router } from "@angular/router";

@Injectable({ providedIn: 'root' })
export class NavigationService {
  constructor(private router: Router) { }

  navigate(route: string): Promise<boolean> {
    return this.router.navigate([route]);
  }

  navigateRelative(subRoute: string, activatedRoute: ActivatedRoute, queryParams?: Params, fragment?: string): Promise<boolean> {
    return this.router.navigate([`../${subRoute}`], { relativeTo: activatedRoute, queryParams: queryParams, fragment: fragment});
  }

  navigateAppend(subRoute: string, activatedRoute: ActivatedRoute, queryParams?: Params, fragment?: string): Promise<boolean> {
    return this.router.navigate([subRoute], { relativeTo: activatedRoute, queryParams: queryParams,fragment: fragment });
  }

  getCurrentRoute(): string {
    return this.router.url;
  }
}
