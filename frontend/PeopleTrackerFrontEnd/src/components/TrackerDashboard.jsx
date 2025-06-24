import { useEffect, useState } from "react";
import api from "../services/api";

function TrackerDashboard() {
  const [stats, setStats] = useState({});

  useEffect(() => {
    api.get("/tracker/summary").then((res) => setStats(res.data));
  }, []);

  return (
    <div>
      <h2>Tracker Summary</h2>
      <p>Total People: {stats.totalPeople}</p>
      <p>Average Age: {stats.averageAge}</p>
    </div>
  );
}

export default TrackerDashboard;
