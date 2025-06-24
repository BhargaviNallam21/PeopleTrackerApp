import axios from "axios";

const PeopleServiceapi = axios.create({
  // baseURL:
  //   "https://peopleservicepeopletracker-hdd6dbgwbmdhgyhu.canadacentral-01.azurewebsites.net",

  baseURL: import.meta.env.VITE_PEOPLE_API,
});

console.log("PeopleAPI", import.meta.env.VITE_PEOPLE_API);
PeopleServiceapi.interceptors.request.use((config) => {
  const token = localStorage.getItem("token"); //get token from local storage
  if (token && !config.url.includes("/login")) {
    config.headers.Authorization = `Bearer ${token}`; //set token
  }
  return config;
});
export default PeopleServiceapi;
