export class Sanitizer {
  static sanitize(body: any): string {
    return `base64:${btoa(JSON.stringify(body))}`;
  }
}
