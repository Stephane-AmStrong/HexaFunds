import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'cronDescription'
})
export class CronDescriptionPipe implements PipeTransform {

  private readonly dayNames = [ 'Sunday', 'Monday',  'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday' ] as const;

  
  /**
   * Parse a CRON expression and extract minute, hour, and days
   * @param cron CRON expression (format: minute hour day month dayOfWeek)
   * @returns Parsed CRON object or null if invalid
   */
  private parseCron(cron: string): {
      minute: number;
      hour: number;
      days: string;
    } | null {

    try {
      if (!cron?.trim()) {
        console.debug('CRON expression is null or empty');
        return null;
      }

      const parts = cron.trim().split(/\s+/);

      if (parts.length < 5) {
        console.debug(`Invalid CRON expression format: ${cron}. Expected at least 5 parts, got ${parts.length}`);
        return null;
      }

      const minute = parseInt(parts[0], 10);

      if (isNaN(minute) || minute < 0 || minute > 59) {
        console.debug(`Invalid minute value in CRON expression: ${cron}`);
        return null;
      }

      const hour = parseInt(parts[1], 10);

      if (isNaN(hour) || hour < 0 || hour > 23) {
        console.debug(`Invalid hour value in CRON expression: ${cron}`);
        return null;
      }

      const daysExpression = parts[4];

      return { minute, hour, days: daysExpression };
    } catch (error) {
      console.warn('Error parsing CRON expression:', cron, error);
      return null;
    }
  }

  /**
   * Parse the days expression and return a human-readable description
   * @param daysExpression Days part of CRON (0-6 where 0=Sunday)
   * @returns Human-readable days description or null if invalid
   */
  private parseDays(daysExpression: string): string | null {
    try {

      if (daysExpression === '*') return 'every day';
      if (daysExpression === '1,2,3,4,5') return 'Monday to Friday';
      if (daysExpression === '0,6') return 'Saturday and Sunday';

      const dayParts = daysExpression.split(',');
      const dayIndices: number[] = [];

      for (const part of dayParts) {

        const index = parseInt(part.trim(), 10);
        
        if (isNaN(index) || index < 0 || index > 6) {
          console.debug(`Invalid day index '${part}' in CRON expression (expected 0-6): ${daysExpression}`);
          return null;
        }

        dayIndices.push(index);
      }

      // Sort and map to day names
      const sortedDays = dayIndices
        .sort((a, b) => a - b)
        .map(i => this.dayNames[i]);

      return `on ${sortedDays.join(', ')}`;
    } catch (error) {
      console.debug('Error parsing days expression:', daysExpression, error);
      return null;
    }
  }

  /**
   * Generates a human-readable description of a CRON expression.
   * Returns the original CRON expression if parsing fails (graceful fallback).
   * 
   * @param cron CRON expression (format: minute hour day month dayOfWeek)
   * @returns Human-readable description (e.g., "14:30 Monday to Friday")
   */

  transform(cron: string): string {
    try {
      if (!cron?.trim()) {
        console.debug('CRON expression is null or empty');
        return cron ?? '';
      }

      const parsedCron = this.parseCron(cron);
      if (!parsedCron) {
        return cron;
      }

      const { minute, hour, days } = parsedCron;

      // Format time with leading zeros
      const timeStr = `${hour.toString().padStart(2, '0')}:${minute.toString().padStart(2, '0')}`;
      
      const daysStr = this.parseDays(days);
      if (!daysStr) {
        return cron;
      }

      return `${timeStr} ${daysStr}`;
    } catch (error) {
      console.warn('Unexpected error getting description for CRON:', cron, error);
      return cron;
    }
  }

}
