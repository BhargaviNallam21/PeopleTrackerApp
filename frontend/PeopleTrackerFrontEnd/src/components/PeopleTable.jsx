import { useEffect, useState } from "react";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import PeopleServiceapi from "../services/PeopleServiceapi";

function PeopleTable() {
  const [people, setPeople] = useState([]);
  const [person, setPerson] = useState({
    fullName: "",
    email: "",
    birthDate: "",
    city: "",
  });
  const [editId, setEditId] = useState(null);
  const [loading, setLoading] = useState(false);
  const [searchTerm, setSearchTerm] = useState("");
  const [currentPage, setCurrentPage] = useState(1);
  const [recordsPerPage] = useState(10);

  const loadPeople = async () => {
    try {
      const res = await PeopleServiceapi.get("/api/People");
      setPeople(res.data);
      setCurrentPage(1);
    } catch (err) {
      toast.error("Error fetching people");
      console.error(err);
    }
  };

  useEffect(() => {
    loadPeople();
  }, []);

  useEffect(() => {
    setCurrentPage(1);
  }, [searchTerm]);

  const handleAdd = async () => {
    if (
      !person.fullName ||
      !person.email ||
      !person.birthDate ||
      !person.city
    ) {
      toast.warn("All fields are required.");
      return;
    }
    try {
      setLoading(true);
      await PeopleServiceapi.post("/api/People/Create", person);
      toast.success("Person added successfully");
      setPerson({ fullName: "", email: "", birthDate: "", city: "" });
      loadPeople();
    } catch (err) {
      toast.error("Error adding person");
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleUpdate = async () => {
    if (
      !person.fullName ||
      !person.email ||
      !person.birthDate ||
      !person.city
    ) {
      toast.warn("All fields are required.");
      return;
    }
    try {
      setLoading(true);
      await PeopleServiceapi.put(`/api/People/${editId}`, person);
      toast.success("Person updated successfully");
      setEditId(null);
      setPerson({ fullName: "", email: "", birthDate: "", city: "" });
      loadPeople();
    } catch (err) {
      toast.error("Error updating person");
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm("Are you sure to delete?")) return;
    try {
      await PeopleServiceapi.delete(`/api/People/${id}`);
      toast.success("Person deleted successfully");
      loadPeople();
    } catch (err) {
      toast.error("Error deleting person");
      console.error(err);
    }
  };

  const handleEdit = (p) => {
    setEditId(p.id);
    setPerson({
      fullName: p.fullName,
      email: p.email,
      birthDate: p.birthDate,
      city: p.city,
    });
  };

  const filteredPeople = people.filter(
    (p) =>
      p.fullName.toLowerCase().includes(searchTerm.toLowerCase()) ||
      p.email.toLowerCase().includes(searchTerm.toLowerCase()) ||
      p.city.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const indexOfLastRecord = currentPage * recordsPerPage;
  const indexOfFirstRecord = indexOfLastRecord - recordsPerPage;
  const currentRecords = filteredPeople.slice(
    indexOfFirstRecord,
    indexOfLastRecord
  );
  const totalPages = Math.max(
    1,
    Math.ceil(filteredPeople.length / recordsPerPage)
  );

  // Ensure currentPage is not out of range
  useEffect(() => {
    if (currentPage > totalPages) {
      setCurrentPage(1);
    }
  }, [filteredPeople, currentPage, totalPages]);

  return (
    <div className="container mt-5">
      <ToastContainer position="top-right" autoClose={3000} hideProgressBar />

      <h2 className="mb-4">People CRUD Operations</h2>

      <input
        type="text"
        className="form-control mb-3"
        placeholder="Search by name, email, or city..."
        value={searchTerm}
        onChange={(e) => setSearchTerm(e.target.value)}
      />

      <div className="card p-3 mb-4">
        <h4>{editId ? "Update Person" : "Add New Person"}</h4>
        <div className="row g-3">
          <div className="col-md-3">
            <input
              type="text"
              className="form-control"
              placeholder="Full Name"
              value={person.fullName}
              onChange={(e) =>
                setPerson({ ...person, fullName: e.target.value })
              }
              required
            />
          </div>
          <div className="col-md-3">
            <input
              type="email"
              className="form-control"
              placeholder="Email"
              value={person.email}
              onChange={(e) => setPerson({ ...person, email: e.target.value })}
              required
            />
          </div>
          <div className="col-md-2">
            <input
              type="date"
              className="form-control"
              value={person.birthDate}
              onChange={(e) =>
                setPerson({ ...person, birthDate: e.target.value })
              }
              required
            />
          </div>
          <div className="col-md-2">
            <input
              type="text"
              className="form-control"
              placeholder="City"
              value={person.city}
              onChange={(e) => setPerson({ ...person, city: e.target.value })}
              required
            />
          </div>
          <div className="col-md-2">
            <button
              className={`btn ${editId ? "btn-warning" : "btn-primary"} w-100`}
              onClick={editId ? handleUpdate : handleAdd}
              disabled={loading}
            >
              {loading ? "Please wait..." : editId ? "Update" : "Add"}
            </button>
          </div>
        </div>
      </div>

      <table className="table table-bordered shadow">
        <thead className="table-light">
          <tr>
            <th>ID</th>
            <th>Full Name</th>
            <th>Email</th>
            <th>Birthday</th>
            <th>City</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {currentRecords.length > 0 ? (
            currentRecords.map((p) => (
              <tr key={p.id}>
                <td>{p.id}</td>
                <td>{p.fullName}</td>
                <td>{p.email}</td>
                <td>{p.birthDate?.substring(0, 10)}</td>
                <td>{p.city}</td>
                <td>
                  <button
                    className="btn btn-sm btn-outline-info me-2"
                    onClick={() => handleEdit(p)}
                  >
                    Edit
                  </button>
                  <button
                    className="btn btn-sm btn-outline-danger"
                    onClick={() => handleDelete(p.id)}
                  >
                    Delete
                  </button>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan="6" className="text-center">
                No people found.
              </td>
            </tr>
          )}
        </tbody>
      </table>

      {/* Pagination Controls */}
      <div className="d-flex justify-content-center mt-3">
        <nav>
          <ul className="pagination">
            <li className={`page-item ${currentPage === 1 ? "disabled" : ""}`}>
              <button
                className="page-link"
                onClick={() => setCurrentPage(currentPage - 1)}
              >
                Previous
              </button>
            </li>

            {(() => {
              const pageNumbers = [];
              const maxPagesToShow = 5;
              let startPage = Math.max(
                1,
                currentPage - Math.floor(maxPagesToShow / 2)
              );
              let endPage = startPage + maxPagesToShow - 1;

              if (endPage > totalPages) {
                endPage = totalPages;
                startPage = Math.max(1, endPage - maxPagesToShow + 1);
              }

              for (let i = startPage; i <= endPage; i++) {
                pageNumbers.push(i);
              }

              return pageNumbers.map((num) => (
                <li
                  key={num}
                  className={`page-item ${currentPage === num ? "active" : ""}`}
                >
                  <button
                    className="page-link"
                    onClick={() => setCurrentPage(num)}
                  >
                    {num}
                  </button>
                </li>
              ));
            })()}

            <li
              className={`page-item ${
                currentPage === totalPages ? "disabled" : ""
              }`}
            >
              <button
                className="page-link"
                onClick={() => setCurrentPage(currentPage + 1)}
              >
                Next
              </button>
            </li>
          </ul>
        </nav>
      </div>

      <p className="text-center mt-2">
        Page {currentPage} of {totalPages}
      </p>
    </div>
  );
}

export default PeopleTable;
