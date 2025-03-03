import React from "react";
import { Button, Card, Col, Form, Row } from "react-bootstrap";
import DownloadReportStyle from "../../stylesheets/DownloadReportStyle.module.css";
import { Captcha } from "../ui-components/Captcha";
import { SideRoundButton } from "../ui-components/SideRoundButton";
import { InvertedButton } from "../ui-components/InvertedButton";

export const DownloadReport = () => {
  return (
    <div>
      <Card className="bg-DBEAB9 m-lg-2 brdr-rad-18">
        <Card.Body>
          <Card.Text className="text-start pt-3">
            <h6 className="color-0053A5 fw-normal fs-4">Download Report</h6>
            <p className="fw-light">
              Please fill out the form below to access the report.
            </p>
          </Card.Text>
          <Card.Text>
            <Form className="text-start">
              <Row className="mb-3">
                <Col lg={6} md={6} sm={12} xs={12}>
                  <Form.Group className="mb-3" controlId="formGridFName">
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
                  <Form.Group controlId="formGridCompany">
                    <Form.Label>Company Name</Form.Label>
                    <Form.Control type="text" />
                  </Form.Group>
                </Col>
              </Row>
              <Row className="mb-3">
                <Col lg={6} md={6} sm={12} xs={12}>
                  <Form.Group
                    as={Col}
                    controlId="formGridFile"
                    className="mb-3"
                  >
                    <Form.Label>Upload File</Form.Label>
                    <Form.Control type="file" />
                  </Form.Group>
                </Col>
                <Col lg={6} md={6} sm={12} xs={12}>
                  <Form.Group as={Col} controlId="formGridFileActive">
                    <Form.Label>Upload File Active</Form.Label>
                    <Form.Control type="file" />
                  </Form.Group>
                </Col>
              </Row>
              <Form.Group controlId="formGridCheck">
                <Form.Label>Checkbox</Form.Label>
                <Form.Check
                  className={`mb-3 pl-2 ${DownloadReportStyle.checkbox_decoration}`}
                  type="checkbox"
                  id="default-checkbox"
                  label="Placeholder"
                />
              </Form.Group>
              <Form.Group
                className="mb-3"
                controlId="exampleForm.ControlTextarea1"
              >
                <Form.Label>Message Field</Form.Label>
                <Form.Control
                  as="textarea"
                  rows={3}
                  placeholder="Placeholder"
                />
              </Form.Group>
              <Row className="mb-3">
                <Col lg={6} md={6} sm={12} xs={12}>
                  <Form.Group
                    as={Col}
                    controlId="formGridDropdown"
                    className="mb-3"
                  >
                    <Form.Label>Dropdown</Form.Label>
                    <Form.Select
                      defaultValue="Choose..."
                      className={`${DownloadReportStyle.custom_select_box}`}
                    >
                      <option>Choose...</option>
                      <option>...</option>
                    </Form.Select>
                  </Form.Group>
                </Col>
                <Col lg={6} md={6} sm={12} xs={12}>
                  <Form.Group as={Col} controlId="formGridDropdown">
                    <Form.Label>Dropdown</Form.Label>
                    <Form.Select
                      defaultValue="Choose..."
                      className={`${DownloadReportStyle.custom_select_box}`}
                    >
                      <option>Choose...</option>
                      <option>...</option>
                    </Form.Select>
                  </Form.Group>
                </Col>
              </Row>
            </Form>
          </Card.Text>
        </Card.Body>
        <div className="d-flex justify-content-between pe-3">
          <Captcha />
          <div className="mt-lg-4">
            <InvertedButton
              name="Download Report"
              siconType="fa-regular"
              siconName="fa-arrow-right-long"
              siconColor="color-0053A5"
            />
          </div>
        </div>
      </Card>
    </div>
  );
};
