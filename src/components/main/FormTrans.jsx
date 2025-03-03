import React, { useState } from "react";
import { Card, Col, Form, Row } from "react-bootstrap";
import FormTransStyle from "../../stylesheets/FormTransStyle.module.css";
import { SideRoundButton } from "../ui-components/SideRoundButton";
import { SuccessPopUp } from "../ui-components/SuccessPopUp";

export const FormTrans = () => {
  const [showAlert, setShowAlert] = useState(false);
  return (
    <>
      <SuccessPopUp showFlag={showAlert} setShowAlertFlag={setShowAlert} />
      <div>
        <Card className="m-lg-2 brdr-rad-18">
          <Card.Body>
            <Card.Text className="text-start pt-3">
              <h6 className="color-0053A5 fw-normal fs-4">
                2024 NACS CEO Summits
              </h6>
              <p className="fw-light">
                A premier, invitation-only experience for CEOS. Please fill out
                the form below to request an invitation to one of the three
                upcoming 2024 NACS CEO summits.
              </p>
            </Card.Text>
            <Card.Text>
              <Form className={`text-start ${FormTransStyle.form_trans}`}>
                <Row className="mb-3">
                  <Col lg={6} md={6} sm={12} xs={12}>
                    <Form.Group controlId="formGridFName">
                      <Form.Label>First Name</Form.Label>
                      <Form.Control type="text" />
                    </Form.Group>
                  </Col>

                  <Col lg={6} md={6} sm={12} xs={12}>
                    <Form.Group controlId="formGridLName">
                      <Form.Label>Last Name</Form.Label>
                      <Form.Control type="text" />
                    </Form.Group>
                  </Col>
                </Row>
                <Row className="mb-3">
                  <Col lg={6} md={6} sm={12} xs={12}>
                    <Form.Group className="mb-3" controlId="formGridEmail">
                      <Form.Label>Email</Form.Label>
                      <Form.Control type="email" />
                    </Form.Group>
                  </Col>
                  <Col lg={6} md={6} sm={12} xs={12}>
                    <Form.Group className="mb-3" controlId="formGridCompany">
                      <Form.Label>Company Name</Form.Label>
                      <Form.Control type="text" />
                    </Form.Group>
                  </Col>
                </Row>

                <Row className="mb-3">
                  <Col lg={6} md={6} sm={12} xs={12}>
                    <Form.Group as={Col} controlId="formGridDropdown">
                      <Form.Label>Dropdown</Form.Label>
                      <Form.Select
                        defaultValue="Choose..."
                        className={`${FormTransStyle.custom_select_box}`}
                      >
                        <option>Choose...</option>
                        <option>...</option>
                      </Form.Select>
                      {/* <Dropdown>
                        <Dropdown.Toggle
                          variant="light"
                          drop="down"
                          id="dropdown-basic"
                          className={`btn dropdown-toggle w-100 d-flex justify-content-between ${FormTransStyle.custom_select_box}`}
                        >
                          Choose...
                        </Dropdown.Toggle>

                        <Dropdown.Menu className="w-100">
                          <Dropdown.Item href="#/action-1">
                            Action
                          </Dropdown.Item>
                          <Dropdown.Item href="#/action-2">
                            Another action
                          </Dropdown.Item>
                          <Dropdown.Item href="#/action-3">
                            Something else
                          </Dropdown.Item>
                        </Dropdown.Menu>
                      </Dropdown> */}
                    </Form.Group>
                  </Col>
                </Row>
              </Form>
            </Card.Text>
          </Card.Body>
          <div className="mb-3">
            <SideRoundButton
              name="Submit Form"
              handleClick={() => setShowAlert(true)}
              className="float-start ms-3"
            />
          </div>
        </Card>
      </div>
    </>
  );
};
