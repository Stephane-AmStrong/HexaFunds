export interface ServerUpdateRequest {
  name: string;
  version: string;
  base: string;
  appName: string;
  baseChain: string[];
  type: string;
  cronStartTime: string;
  cronStopTime: string;
  configs: string[];
  dictionaries: string[];
  runMode: string;
  configPaths: string[];
  dictionaryPaths: string[];
  tags: string[];
}