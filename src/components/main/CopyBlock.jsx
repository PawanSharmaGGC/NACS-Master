import React from "react";
import { Card } from "react-bootstrap";
import { BackToLog } from "../ui-components/BackToLog";
import CopyBlockStyle from "../../stylesheets/CopyBlockStyle.module.css";

export const CopyBlock = () => {
  return (
    <div>
      <Card style={{}} className={`p-3 border-0 ${CopyBlockStyle.main_card}`}>
        <Card className={`border-0 ${CopyBlockStyle.section_card}`}>
          <Card.Body className="p-0 ">
            <div className=" text-start mb-3">
              Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc
              tristique libero vitae metus euismod, ut rutrum lacus commodo.
              Proin eu nisl lobortis, lobortis quam at, aliquet tellus. Vivamus
              mollis ex sed nisi faucibus finibus.
            </div>
            <div className="text-start mb-3">
              Praesent nisi ipsum, finibus scelerisque risus at, laoreet
              eleifend magna. Nunc sem ipsum, euismod ut metus id, ultrices
              convallis erat.
            </div>
          </Card.Body>
        </Card>
        {/* <div className="mt-5">
          <BackToLog />
        </div> */}
      </Card>
    </div>
  );
};
