export type ChartCardConfig = "area-chart" | "bar-chart" | "line-chart"| "hexagon-chart";

export interface BaseChartConfig {
  title: string;
  rows: number;
  cols: number;
}

export interface NumericChartConfig extends BaseChartConfig {
  contentType: "area-chart" | "bar-chart" | "line-chart";
  seriesData: SeriesData[];
}

export interface HexagonChartConfig extends BaseChartConfig {
  contentType: "honeycomb-chart";
  seriesData: DataPoint[];
}

export type ChartConfig = NumericChartConfig | HexagonChartConfig;

export interface SeriesData {
  name?: string;
  data: number[];
}

export interface DataPoint {
  name?: string;
  abbreviation?: string;
  x: number;
  y: number;
  value: number;
}
