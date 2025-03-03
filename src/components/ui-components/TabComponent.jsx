import React, { useRef } from "react";
import TabStyle from "../../stylesheets/TabStyle.module.css";
import { Nav, Tab, Tabs } from "react-bootstrap";
import { TableComponent } from "./TableComponent";
import { CopyBlock } from "../main/CopyBlock";
import { EyebrowTitle } from "./EyebrowTitle";

export const TabComponent = () => {
  const containerRef = useRef(null);

  const scrollLeft = () => {
    if (containerRef.current) {
      containerRef.current.scrollBy({ left: -200, behavior: "smooth" });
    }
  };

  const scrollRight = () => {
    if (containerRef.current) {
      containerRef.current.scrollBy({ left: 200, behavior: "smooth" });
    }
  };
  return (
    <div className="mb-3">
      <Tab.Container id="uncontrolled-tab-example" defaultActiveKey="profile">
        <div className="d-flex mb-3">
          <Nav
            ref={containerRef}
            variant="underline"
            className={`border_bottom_grey ${TabStyle.tabs}`}
          >
            <Nav.Item>
              <Nav.Link eventKey="home">About</Nav.Link>
            </Nav.Item>
            <Nav.Item>
              <Nav.Link eventKey="profile">Agenda</Nav.Link>
            </Nav.Item>
            <Nav.Item>
              <Nav.Link eventKey="home1">About</Nav.Link>
            </Nav.Item>
            <Nav.Item>
              <Nav.Link eventKey="home2">Agenda</Nav.Link>
            </Nav.Item>
            <Nav.Item>
              <Nav.Link eventKey="home3">Agenda</Nav.Link>
            </Nav.Item>
            <Nav.Item>
              <Nav.Link eventKey="home4">Agenda</Nav.Link>
            </Nav.Item>
            <Nav.Item>
              <Nav.Link eventKey="home5">Agenda</Nav.Link>
            </Nav.Item>
            <Nav.Item>
              <Nav.Link eventKey="home6">Agenda</Nav.Link>
            </Nav.Item>
            <Nav.Item>
              <Nav.Link eventKey="home7">Agenda</Nav.Link>
            </Nav.Item>
            <Nav.Item>
              <Nav.Link eventKey="home8">Agenda</Nav.Link>
            </Nav.Item>
            <Nav.Item>
              <Nav.Link eventKey="home9">Agenda</Nav.Link>
            </Nav.Item>
            <Nav.Item>
              <Nav.Link eventKey="home10">Agenda</Nav.Link>
            </Nav.Item>
            <Nav.Item>
              <Nav.Link eventKey="home11">Agenda</Nav.Link>
            </Nav.Item>
            <Nav.Item>
              <Nav.Link eventKey="home12">Agenda</Nav.Link>
            </Nav.Item>
            <Nav.Item>
              <Nav.Link eventKey="home13">Agenda</Nav.Link>
            </Nav.Item>
          </Nav>

          <div className="text-center h-fit p-2 border brdr-r-rad-18">
            <i
              onClick={scrollLeft}
              className="fa-regular fa-angle-left fa-lg color-0053A5 mb-4 pointer"
            ></i>

            <i
              onClick={scrollRight}
              className="fa-regular fa-angle-right fa-lg color-0053A5 pointer"
            ></i>
          </div>
        </div>

        <Tab.Content>
          <Tab.Pane eventKey="home">
            <CopyBlock />
          </Tab.Pane>
          <Tab.Pane eventKey="profile">
            <EyebrowTitle leftBorderColor="border_left_green" title="agenda" />
            <p className="h3 color-002569 text-start mb-4">
              Agenda Subtitle Here
            </p>
            <p className="m-0 text-start">
              Updated as of May 1, 2024; content subject to change.
            </p>
            <div className="mt-4">
              <TableComponent />
            </div>
          </Tab.Pane>
          <Tab.Pane eventKey="home1">
            <CopyBlock />
          </Tab.Pane>
          <Tab.Pane eventKey="home2">
            <CopyBlock />
          </Tab.Pane>
          <Tab.Pane eventKey="home3">
            <CopyBlock />
          </Tab.Pane>
          <Tab.Pane eventKey="home4">
            <CopyBlock />
          </Tab.Pane>
          <Tab.Pane eventKey="home5">
            <CopyBlock />
          </Tab.Pane>
          <Tab.Pane eventKey="home6">
            <CopyBlock />
          </Tab.Pane>
          <Tab.Pane eventKey="home7">
            <CopyBlock />
          </Tab.Pane>
          <Tab.Pane eventKey="home8">
            <CopyBlock />
          </Tab.Pane>
          <Tab.Pane eventKey="home9">
            <CopyBlock />
          </Tab.Pane>
          <Tab.Pane eventKey="home10">
            <CopyBlock />
          </Tab.Pane>
          <Tab.Pane eventKey="home11">
            <CopyBlock />
          </Tab.Pane>
          <Tab.Pane eventKey="home12">
            <CopyBlock />
          </Tab.Pane>
          <Tab.Pane eventKey="home13">
            <CopyBlock />
          </Tab.Pane>
        </Tab.Content>
      </Tab.Container>
    </div>
  );
};
