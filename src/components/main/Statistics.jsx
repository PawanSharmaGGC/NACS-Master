import React from "react";
import StatisticsStyle from "../../stylesheets/StatisticsStyle.module.css";
import { Card } from "react-bootstrap";
import { BackToLog } from "../ui-components/BackToLog";

export const Statistics = () => {
  return (
    <div>
      <Card className={`p-3 ${StatisticsStyle.main_card}`}>
        <Card.Body className="p-1 mb-4">
          <h4 className="text-start pb-4 color-002569">By The Numbers</h4>
          <div className={`${StatisticsStyle.by_the_num}`}>
            <div className={`${StatisticsStyle.by_the_num_blocks}`}>
              <p className="m-0 text-body-tertiary fs-sm-6 fs-lg-5 fs-xl-5">
                Retail Gas Price (AAA)
              </p>
              <p className="m-0">
                <span className="fs-2 color-0053A5 fw-semibold">$3.491</span>
                <span className="fs-4 color-0053A5">/gal</span>
              </p>
              <p className="m-0">
                <span>
                  <i className="pe-1 color-DC241F fa-regular fa-arrow-down"></i>
                </span>
                <span className="pe-1 color-DC241F">-0.012</span>
                <span className="text-body-tertiary">since yesterday</span>
              </p>
            </div>
            <div className={`${StatisticsStyle.by_the_num_blocks}`}>
              <p className="m-0 text-body-tertiary fs-sm-6 fs-lg-5 fs-xl-5">
                Retail Gas Price (AAA)
              </p>
              <p className="m-0">
                <span className="fs-2 color-0053A5 fw-semibold">$3.491</span>
                <span className="fs-4 color-0053A5">/gal</span>
              </p>
              <p className="m-0">
                <span>
                  <i className="pe-1 color-DC241F fa-regular fa-arrow-down"></i>
                </span>
                <span className="pe-1 color-DC241F">-0.012</span>
                <span className="text-body-tertiary">since yesterday</span>
              </p>
            </div>
            <div className={`${StatisticsStyle.by_the_num_blocks}`}>
              <p className="m-0 text-body-tertiary fs-sm-6 fs-lg-5 fs-xl-5">
                Retail Gas Price (AAA)
              </p>
              <p className="m-0">
                <span className="fs-2 color-0053A5 fw-semibold">$3.491</span>
                <span className="fs-4 color-0053A5">/gal</span>
              </p>
              <p className="m-0">
                <span>
                  <i className="pe-1 color-DC241F fa-regular fa-arrow-down"></i>
                </span>
                <span className="pe-1 color-DC241F">-0.012</span>
                <span className="text-body-tertiary">since yesterday</span>
              </p>
            </div>
            <div className={`${StatisticsStyle.by_the_num_blocks}`}>
              <p className="m-0 text-body-tertiary fs-sm-6 fs-lg-5 fs-xl-5">
                Retail Gas Price (AAA)
              </p>
              <p className="m-0">
                <span className="fs-2 color-0053A5 fw-semibold">$3.491</span>
                <span className="fs-4 color-0053A5">/gal</span>
              </p>
              <p className="m-0">
                <span>
                  <i className="pe-1 color-62BD19 fa-regular fa-arrow-up"></i>
                </span>
                <span className="pe-1 color-62BD19">+0.012</span>
                <span className="text-body-tertiary">since yesterday</span>
              </p>
            </div>
          </div>
        </Card.Body>
        <Card.Body className="p-1">
          <h4 className="text-start pb-4 color-002569">Factor Id</h4>
          <div className={`${StatisticsStyle.factor_id}`}>
            <div className={`${StatisticsStyle.factor_id_blocks}`}>
              <div className={`${StatisticsStyle.factor_id_flex_blocks}`}>
                <p className="m-0 text-body-tertiary fs-sm-6 fs-lg-5 fs-xl-5">
                  Retail Gas Price (AAA)
                </p>
                <p className="m-0">
                  <span className="fs-2 color-0053A5 fw-semibold">$3.491</span>
                  <span className="fs-4 color-0053A5">/gal</span>
                </p>
              </div>
              <div>
                <span className="fs-sm-6 fs-lg-4 fs-xl-4 text-body-tertiary">
                  The average sales per store, per month for commissary in 2023.
                </span>
              </div>
            </div>
            <div className={`${StatisticsStyle.factor_id_blocks}`}>
              <div className={`${StatisticsStyle.factor_id_flex_blocks}`}>
                <p className="m-0 text-body-tertiary fs-sm-6 fs-lg-5 fs-xl-5">
                  Retail Gas Price (AAA)
                </p>
                <p className="m-0">
                  <span className="fs-2 color-0053A5 fw-semibold">$3.491</span>
                  <span className="fs-4 color-0053A5">/gal</span>
                </p>
              </div>
              <div>
                <span className="fs-sm-6 fs-lg-4 fs-xl-4 text-body-tertiary">
                  The average sales per store, per month for commissary in 2023.
                </span>
              </div>
            </div>
            <div className={`${StatisticsStyle.factor_id_blocks}`}>
              <div className={`${StatisticsStyle.factor_id_flex_blocks}`}>
                <p className="m-0 text-body-tertiary fs-sm-6 fs-lg-5 fs-xl-5">
                  Retail Gas Price (AAA)
                </p>
                <p className="m-0">
                  <span className="fs-2 color-0053A5 fw-semibold">$3.491</span>
                  <span className="fs-4 color-0053A5">/gal</span>
                </p>
              </div>
              <div>
                <span className="fs-sm-6 fs-lg-4 fs-xl-4 text-body-tertiary">
                  The average sales per store, per month for commissary in 2023.
                </span>
              </div>
            </div>
            <div className={`${StatisticsStyle.factor_id_blocks}`}>
              <div className={`${StatisticsStyle.factor_id_flex_blocks}`}>
                <p className="m-0 text-body-tertiary fs-sm-6 fs-lg-5 fs-xl-5">
                  Retail Gas Price (AAA)
                </p>
                <p className="m-0">
                  <span className="fs-2 color-0053A5 fw-semibold">$3.491</span>
                  <span className="fs-4 color-0053A5">/gal</span>
                </p>
              </div>
              <div>
                <span className="fs-sm-6 fs-lg-4 fs-xl-4 text-body-tertiary">
                  The average sales per store, per month for commissary in 2023.
                </span>
              </div>
            </div>
          </div>
        </Card.Body>

        <div className="mt-5">
          <BackToLog />
        </div>
      </Card>
    </div>
  );
};
