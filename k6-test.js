import http from 'k6/http';
import { sleep } from 'k6';

export default function () {
    http.get("https://localhost:8081/weather-forecast");
    //and so on
}

export const options = {
    vus: 100,
    duration: "30s"
}