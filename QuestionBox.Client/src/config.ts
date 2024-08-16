import * as appConfig from "../../AppConfig.json";

const serverUrl =
  process.env.NODE_ENV === "development"
    ? appConfig.ServerDevUrl
    : appConfig.ApiUrl;

console.log(serverUrl);
export const questionApiUrl = serverUrl + "api/questions";