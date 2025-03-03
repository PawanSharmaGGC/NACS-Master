import React, { useEffect } from "react";
import AvatarStyle from "../../stylesheets/AvatarStyle.module.css";

export const Avatars = ({ size, isName = false, imgSrc = "", name = "" }) => {
  useEffect(() => {
    if (isName) {
      const txt = document.getElementById("avatarName");
      txt.style.width = size + "px";
      txt.style.height = size + "px";
      txt.style.fontSize = 0.4375 * size + "px";
    }
  }, [size, isName]);

  const nameAvatarIcon = (name) => {
    let fName = name.split(" ")[0][0].toUpperCase();
    let lName = name.split(" ")[1][0].toUpperCase();

    return fName + lName;
  };

  return (
    <div>
      <div>
        {!isName && (
          <img
            id="avatarImage"
            className="rounded-circle"
            width={size}
            height={size}
            src={imgSrc}
            alt={<i className="fa-solid fa-user"></i>}
          />
        )}
        {isName && (
          <div id="avatarName" className={`${AvatarStyle.profile_name}`}>
            <span className="v-align-middle">{nameAvatarIcon(name)}</span>
          </div>
        )}
      </div>
    </div>
  );
};
