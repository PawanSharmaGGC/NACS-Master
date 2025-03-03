import React from "react";
import { Card, Form, InputGroup } from "react-bootstrap";
import SubscribeComponentStyle from "../../stylesheets/SubscribeComponentStyle.module.css";

export const SubscribeComponent = () => {
  return (
    <Card className="flex-card border border-0 mb-3">
      <Card.Body className={`p-0 ${SubscribeComponentStyle.main_card_body}`}>
        <div className="text-start p-4">
          <div className="mb-4 fs-4 color-002569">Subscribe to NACS Daily</div>
          <div className="fs-5 mb-2">
            <InputGroup
              className={`mb-3 ${SubscribeComponentStyle.subscribe_input_grp}`}
            >
              <Form.Control
                placeholder="Email"
                aria-label="Recipient's username"
                aria-describedby="basic-addon2"
                className={`border border-0 ${SubscribeComponentStyle.input_elem}`}
              />
              <InputGroup.Text
                className={`border border-0 ps-5 pe-5 bg-0053A5 color-FFFFFF pointer ${SubscribeComponentStyle.subscribe_input_txt}`}
                id="basic-addon2"
              >
                Sign Up
              </InputGroup.Text>
            </InputGroup>
          </div>
        </div>
      </Card.Body>
    </Card>
  );
};
