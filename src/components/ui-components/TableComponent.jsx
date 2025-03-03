import React, { useEffect, useState } from "react";
import TableStyle from "../../stylesheets/TableStyle.module.css";
import { PaginationNumber } from "./PaginationNumber";

export const TableComponent = () => {
  const [page, setPage] = useState([]);
  const [activePage, setActivePage] = useState(1);

  useEffect(() => {
    const tempArr = [];
    for (let i = 1; i <= 100; i++) {
      tempArr.push(i);
    }
    setPage(tempArr);
  }, []);

  const next = () => {
    let count = activePage;
    if (count < page.length) {
      count += 1;
      setActivePage(count);
    }
  };
  const previous = () => {
    let count = activePage;
    if (count > 1) {
      count -= 1;
      setActivePage(count);
    }
  };
  return (
    <div>
      <table className={`table ${TableStyle.custom_table_striped}`}>
        <thead>
          <tr>
            <th scope="col">#</th>
            <th scope="col">First</th>
            <th scope="col">Last</th>
            <th scope="col">Handle</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <th scope="row">1</th>
            <td>Mark</td>
            <td>Otto</td>
            <td>@mdo</td>
          </tr>
          <tr>
            <th scope="row">2</th>
            <td>Jacob</td>
            <td>Thornton</td>
            <td>@fat</td>
          </tr>
          <tr>
            <th scope="row">3</th>
            <td>Larry the Bird</td>
            <td>Thomas</td>
            <td>@twitter</td>
          </tr>
        </tbody>
      </table>
      <PaginationNumber
        data={page}
        activePage={activePage}
        next={next}
        previous={previous}
        setActivePage={setActivePage}
      />
    </div>
  );
};
