import axios from "axios";

const api = axios.create({
  // baseURL: "https://authservicepeopletracker-cqfzdacrcrbcdaax.canadacentral-01.azurewebsites.net", // Prod URL

  baseURL: import.meta.env.VITE_AUTH_API,
});
console.log("Auth API:", import.meta.env.VITE_AUTH_API);
// Add token to all requests except /login
api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token && !config.url.includes("/api/Auth/login")) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;
