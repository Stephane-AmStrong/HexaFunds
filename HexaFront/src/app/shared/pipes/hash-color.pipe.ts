import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'hashColor'
})
export class HashColorPipe implements PipeTransform {

  private readonly FNV_OFFSET_BASIS = 2166136261;
  private readonly FNV_PRIME = 16777619;

  transform(input: string): string {
    if(!input || input.length === 0) {
      return '';
    }

    const hash = this.hashString(input);
    const hue = Math.abs(hash % 360);

    const saturation = 70;
    const lightness = 55;

    return `hsl(${hue}, ${saturation}%, ${lightness}%)`;
  }

  private hashString(input: string): number {
    let hash = this.FNV_OFFSET_BASIS;
    for (let i = 0; i < input.length; i++) {
      hash ^= input.charCodeAt(i);
      hash = (hash * this.FNV_PRIME) >>> 0;
    }
    return hash;
  }

}
