import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";


function LoginForm() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleLogin = async () => {
    if (!username || !password) {
      alert("Username and password are required.");
      return;
    }

    try {
      setLoading(true);
      const response = await api.post("/api/Auth/login", {
        username,
        passwordHash: password, // match with backend param
      });
      const jwt = response.data?.token?.token;

      if (jwt) {
        localStorage.setItem("token", jwt);

        console.log("Full response:", response.data);
        navigate("/people");
      } else {
        alert("Login failed: Token not found in response.");
      }
    } catch (error) {
      console.error("Login error:", error);
      alert("Login failed: Invalid credentials or server error.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container mt-5" style={{ maxWidth: "400px" }}>
      <div className="card shadow">
        <div className="card-body">
          <h3 className="card-title text-center mb-4">Login</h3>
          <div className="mb-3">
            <input
              type="text"
              className="form-control"
              placeholder="Username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              autoFocus
            />
          </div>
          <div className="mb-3">
            <input
              type="password"
              className="form-control"
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>
          <button
            className="btn btn-primary w-100"
            onClick={handleLogin}
            disabled={loading}
          >
            {loading ? "Logging in..." : "Login"}
          </button>
        </div>
      </div>
    </div>
  );
}

export default LoginForm;
