import React, { useState } from "react";
import { Card, Col, Form, Row } from "react-bootstrap";
import { InvertedButton } from "../ui-components/InvertedButton";
import { SuccessPopUp } from "../ui-components/SuccessPopUp";

export const SignInForm = () => {
  const [showAlert, setShowAlert] = useState(false);

  return (
    <>
      <SuccessPopUp showFlag={showAlert} setShowAlertFlag={setShowAlert} />
      <div>
        <Card className="bg-DBEAB9 h-100 m-lg-2 brdr-rad-18">
          <Card.Body>
            <Card.Text className="text-start pt-3">
              <h6 className="color-0053A5 fw-normal fs-4">Sign Up</h6>
              <p className="fw-light">
                Please fill out the form below to sign up for the webinar.
              </p>
            </Card.Text>
            <Card.Text>
              <Form className="text-start">
                <Row className="mb-3">
                  <Col lg={12} md={12} sm={12} xs={12}>
                    <Form.Group controlId="formGridFName">
                      <Form.Label>First Name</Form.Label>
                      <Form.Control type="text" />
                    </Form.Group>
                  </Col>

                  <Col lg={12} md={12} sm={12} xs={12}>
                    <Form.Group controlId="formGridLName">
                      <Form.Label>Last Name</Form.Label>
                      <Form.Control type="text" />
                    </Form.Group>
                  </Col>
                </Row>
                <Row className="mb-3">
                  <Col lg={12} md={12} sm={12} xs={12}>
                    <Form.Group className="mb-3" controlId="formGridEmail">
                      <Form.Label>Email</Form.Label>
                      <Form.Control type="email" />
                    </Form.Group>
                  </Col>
                  <Col lg={12} md={12} sm={12} xs={12}>
                    <Form.Group className="" controlId="formGridCompany">
                      <Form.Label>Company Name</Form.Label>
                      <Form.Control type="text" />
                    </Form.Group>
                  </Col>
                </Row>
              </Form>
            </Card.Text>
          </Card.Body>
          <div className="mb-3">
            <InvertedButton
              name="Sign Up"
              siconType="fa-regular"
              siconName="fa-arrow-right-long"
              siconColor="color-0053A5"
              handleClick={() => setShowAlert(true)}
            />
          </div>
        </Card>
      </div>
    </>
  );
};
