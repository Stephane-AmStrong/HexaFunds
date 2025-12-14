import { CronDescriptionPipe } from './cron-description.pipe';

describe('CronDescriptionPipe', () => {
  let pipe: CronDescriptionPipe;

  beforeEach(() => {
    pipe = new CronDescriptionPipe();
  });

  it('should create an instance', () => {
    expect(pipe).toBeTruthy();
  });

  describe('transform', () => {
    describe('with valid CRON expressions', () => {
      it('should return "02:18 every day" for "18 02 * * *"', () => {
        const result = pipe.transform('18 02 * * *');
        expect(result).toBe('02:18 every day');
      });

      it('should return "04:00 Monday to Friday" for "00 04 * * 1,2,3,4,5"', () => {
        const result = pipe.transform('00 04 * * 1,2,3,4,5');
        expect(result).toBe('04:00 Monday to Friday');
      });

      it('should return "16:30 Saturday and Sunday" for "30 16 * * 0,6"', () => {
        const result = pipe.transform('30 16 * * 0,6');
        expect(result).toBe('16:30 Saturday and Sunday');
      });

      it('should return "00:00 every day" for "00 00 * * *"', () => {
        const result = pipe.transform('00 00 * * *');
        expect(result).toBe('00:00 every day');
      });

      it('should return "23:45 on Monday" for "45 23 * * 1"', () => {
        const result = pipe.transform('45 23 * * 1');
        expect(result).toBe('23:45 on Monday');
      });

      it('should return "09:15 on Monday, Wednesday, Friday" for "15 09 * * 1,3,5"', () => {
        const result = pipe.transform('15 09 * * 1,3,5');
        expect(result).toBe('09:15 on Monday, Wednesday, Friday');
      });
    });

    describe('with invalid minute or hour', () => {
      it('should return original CRON for "99 99 * * *"', () => {
        const cron = '99 99 * * *';
        const result = pipe.transform(cron);
        expect(result).toBe(cron);
      });

      it('should return original CRON for "60 25 * * *"', () => {
        const cron = '60 25 * * *';
        const result = pipe.transform(cron);
        expect(result).toBe(cron);
      });

      it('should return original CRON for "-1 10 * * *"', () => {
        const cron = '-1 10 * * *';
        const result = pipe.transform(cron);
        expect(result).toBe(cron);
      });

      it('should return original CRON for "30 -5 * * *"', () => {
        const cron = '30 -5 * * *';
        const result = pipe.transform(cron);
        expect(result).toBe(cron);
      });
    });

    describe('with invalid format', () => {
      it('should return original CRON for "invalid"', () => {
        const cron = 'invalid';
        const result = pipe.transform(cron);
        expect(result).toBe(cron);
      });

      it('should return original CRON for "* *"', () => {
        const cron = '* *';
        const result = pipe.transform(cron);
        expect(result).toBe(cron);
      });

      it('should return original CRON for "10 20"', () => {
        const cron = '10 20';
        const result = pipe.transform(cron);
        expect(result).toBe(cron);
      });

      it('should return original CRON for "a b c d e"', () => {
        const cron = 'a b c d e';
        const result = pipe.transform(cron);
        expect(result).toBe(cron);
      });
    });

    describe('with null or empty CRON', () => {
      it('should return empty string for null', () => {
        const result = pipe.transform(null as any);
        expect(result).toBe('');
      });

      it('should return empty string for empty string', () => {
        const result = pipe.transform('');
        expect(result).toBe('');
      });

      it('should return original string for whitespace only', () => {
        const cron = '   ';
        const result = pipe.transform(cron);
        expect(result).toBe(cron);
      });
    });

    describe('with invalid day indices', () => {
      it('should return original CRON for "30 14 * * 7"', () => {
        const cron = '30 14 * * 7';
        const result = pipe.transform(cron);
        expect(result).toBe(cron);
      });

      it('should return original CRON for "30 14 * * 99"', () => {
        const cron = '30 14 * * 99';
        const result = pipe.transform(cron);
        expect(result).toBe(cron);
      });

      it('should return original CRON for "30 14 * * -1"', () => {
        const cron = '30 14 * * -1';
        const result = pipe.transform(cron);
        expect(result).toBe(cron);
      });

      it('should return original CRON for "30 14 * * a,b,c"', () => {
        const cron = '30 14 * * a,b,c';
        const result = pipe.transform(cron);
        expect(result).toBe(cron);
      });
    });

    describe('edge cases', () => {
      it('should handle multiple spaces between parts', () => {
        const result = pipe.transform('30    14    *    *    1,2,3,4,5');
        expect(result).toBe('14:30 Monday to Friday');
      });

      it('should handle leading and trailing spaces', () => {
        const result = pipe.transform('  30 14 * * 1,2,3,4,5  ');
        expect(result).toBe('14:30 Monday to Friday');
      });

      it('should handle unsorted day indices', () => {
        const result = pipe.transform('30 14 * * 5,1,3');
        expect(result).toBe('14:30 on Monday, Wednesday, Friday');
      });

      it('should handle duplicate day indices', () => {
        const result = pipe.transform('30 14 * * 1,1,3,3');
        expect(result).toContain('Monday');
        expect(result).toContain('Wednesday');
      });
    });
  });

  describe('console logging', () => {
    beforeEach(() => {
      spyOn(console, 'debug');
      spyOn(console, 'warn');
    });

    it('should log debug message for null/empty CRON', () => {
      pipe.transform('');
      expect(console.debug).toHaveBeenCalledWith('CRON expression is null or empty');
    });

    it('should log debug message for invalid format', () => {
      pipe.transform('* *');
      expect(console.debug).toHaveBeenCalledWith(
        jasmine.stringContaining('Invalid CRON expression format')
      );
    });

    it('should log debug message for invalid minute', () => {
      pipe.transform('99 14 * * *');
      expect(console.debug).toHaveBeenCalledWith(
        jasmine.stringContaining('Invalid minute value')
      );
    });

    it('should log debug message for invalid hour', () => {
      pipe.transform('30 25 * * *');
      expect(console.debug).toHaveBeenCalledWith(
        jasmine.stringContaining('Invalid hour value')
      );
    });

    it('should log debug message for invalid day index', () => {
      pipe.transform('30 14 * * 7');
      expect(console.debug).toHaveBeenCalledWith(
        jasmine.stringContaining('Invalid day index')
      );
    });
  });
});
