import { HexagonChartConfig, DataPoint, NumericChartConfig } from '../chart-config';

export const MOCK_NumericChartConfig: NumericChartConfig = {
  title: 'CPU Usage',
  rows: 1,
  cols: 2,
  contentType: 'line-chart',
  seriesData: [
    {
      name: 'Server A',
      data: [10, 20, 30, 40, 35, 25],
    },
    {
      name: 'Server B',
      data: [15, 25, 20, 30, 45, 40],
    },
  ],
};

export const MOCK_HexagonChartConfig: HexagonChartConfig[] = [
  {
    title: 'Card 6',
    cols: 5,
    rows: 2,
    seriesData: [
      { name: 'Oregon', abbreviation: 'OR', x: 5, y: 5, value: 4233358 },
      { name: 'South Dakota', abbreviation: 'SD', x: 5, y: 6, value: 7812880 },
      { name: 'Vermont', abbreviation: 'VT', x: 5, y: 7, value: 647464 },
      { name: 'Washington', abbreviation: 'WA', x: 6, y: 8, value: 7812880 },
    ],
    contentType: 'honeycomb-chart',
  },
];
