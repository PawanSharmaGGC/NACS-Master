import React from "react";
import { Card, Col, Row } from "react-bootstrap";
import inline from "../../assests/images/inline.png";
import { BackToLog } from "../ui-components/BackToLog";
import InlineRelatedPostStyle from "../../stylesheets/InlineRelatedPostStyle.module.css";
import { DateTimeArticleSummary } from "../ui-components/DateTimeArticleSummary";

export const InlineRelatedPost = () => {
  return (
    <div>
      <Card
        className={`p-3 border border-0 ${InlineRelatedPostStyle.main_card}`}
      >
        <Card className={`mt-4 ${InlineRelatedPostStyle.section_card}`}>
          <Card.Body className="p-5 pe-0 ps-0">
            <Row>
              <Col lg={4} sm={12} className="mb-2">
                <div className="d-flex">
                  <div className="pt-1 pt-lg-0">
                    <img className="rounded" src={inline} alt="" width={120} />
                  </div>
                  <div className="w-100 align-middle text-start ps-4 fs-6">
                    <p className="mb-0 mb-lg-2  text-body-tertiary">
                      Related Post
                    </p>
                    <p className=" fs-5 mb-0 mb-lg-2 color-0053A5">
                      My Favorite Resturant Served Gas - Episode 434
                    </p>

                    <DateTimeArticleSummary
                      textColor="color-262d61"
                      date="24 March 2024"
                      article="10min Listen"
                      fontSize="fs-6"
                    />
                  </div>
                </div>
              </Col>
              <Col lg={4} sm={12} className="mb-2">
                <div className="d-flex">
                  <div className="pt-1 pt-lg-0">
                    <img className="rounded" src={inline} alt="" width={120} />
                  </div>
                  <div className="w-100 align-middle text-start ps-4 fs-6">
                    <p className="mb-0 mb-lg-2  text-body-tertiary">
                      Related Post
                    </p>
                    <p className=" fs-5 mb-0 mb-lg-2  color-0053A5">
                      My Favorite Resturant Served Gas - Episode 434
                    </p>

                    <DateTimeArticleSummary
                      textColor="color-262d61"
                      date="24 March 2024"
                      article="10min Listen"
                      fontSize="fs-6"
                    />
                  </div>
                </div>
              </Col>
              <Col lg={4} sm={12} className="mb-2">
                <div className="d-flex">
                  <div className="pt-1 pt-lg-0">
                    <img className="rounded" src={inline} alt="" width={120} />
                  </div>
                  <div className="w-100 align-middle text-start ps-4 fs-6">
                    <p className="mb-0 mb-lg-2  text-body-tertiary">
                      Related Post
                    </p>
                    <p className=" fs-5 mb-0 mb-lg-2  color-0053A5">
                      My Favorite Resturant Served Gas - Episode 434
                    </p>

                    <DateTimeArticleSummary
                      textColor="color-262d61"
                      date="24 March 2024"
                      article="10min Listen"
                      fontSize="fs-6"
                    />
                  </div>
                </div>
              </Col>
              <Col lg={4} sm={12} className="mb-2">
                <div className="d-flex">
                  <div className="pt-1 pt-lg-0">
                    <img className="rounded" src={inline} alt="" width={120} />
                  </div>
                  <div className="w-100 align-middle text-start ps-4 fs-6">
                    <p className="mb-0 mb-lg-2  text-body-tertiary">
                      Related Post
                    </p>
                    <p className=" fs-5 mb-0 mb-lg-2  color-0053A5">
                      My Favorite Resturant Served Gas - Episode 434
                    </p>

                    <DateTimeArticleSummary
                      textColor="color-262d61"
                      date="24 March 2024"
                      article="10min Listen"
                      fontSize="fs-6"
                    />
                  </div>
                </div>
              </Col>
              <Col lg={4} sm={12} className="mb-2">
                <div className="d-flex">
                  <div className="pt-1 pt-lg-0">
                    <img className="rounded" src={inline} alt="" width={120} />
                  </div>
                  <div className="w-100 align-middle text-start ps-4 fs-6">
                    <p className="mb-0 mb-lg-2  text-body-tertiary">
                      Related Post
                    </p>
                    <p className=" fs-5 mb-0 mb-lg-2  color-0053A5">
                      My Favorite Resturant Served Gas - Episode 434
                    </p>

                    <DateTimeArticleSummary
                      textColor="color-262d61"
                      date="24 March 2024"
                      article="10min Listen"
                      fontSize="fs-6"
                    />
                  </div>
                </div>
              </Col>
            </Row>
          </Card.Body>
        </Card>
      </Card>
    </div>
  );
};
