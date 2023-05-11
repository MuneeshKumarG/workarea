import styles from "@/styles/DependencyMapping.module.css";
import { ButtonComponent } from "@syncfusion/ej2-react-buttons";
import { ListViewComponent } from "@syncfusion/ej2-react-lists";
import { ComboBoxComponent } from "@syncfusion/ej2-react-dropdowns";
import { TextBoxComponent } from "@syncfusion/ej2-react-inputs";
import { CheckBoxComponent } from "@syncfusion/ej2-react-buttons";
import { useState } from "react";
import { useRef } from "react";
import Image from "next/image";
import React from "react";

let fieldName = "State";
let fieldChildrenGroupNames: string[] = [];
let fieldValues: any[] = [
  {
    isMapped: false,
    isChecked: false,
    name: "TamilNadu",
    index: 0,
    parent: null,
    children: [],
  },
  {
    isMapped: false,
    isChecked: false,
    name: "AndhraPradesh",
    index: 1,
    parent: null,
    children: [],
  },
  {
    isMapped: false,
    isChecked: false,
    name: "Kerala",
    index: 2,
    parent: null,
    children: [],
  },
  {
    isMapped: false,
    isChecked: false,
    name: "Karnataka",
    index: 3,
    parent: null,
    children: [],
  },
  {
    isMapped: false,
    isChecked: false,
    name: "Telangana",
    index: 4,
    parent: null,
    children: [],
  },
];

let existingFields: any[] = [
  {
    name: "Country",
    values: [
      {
        name: "India",
        isChecked: false,
        index: 0,
        isMapped: false,
        parent: null,
        children: [],
      },
      {
        name: "USA",
        isChecked: false,
        index: 1,
        isMapped: false,
        parent: null,
        children: [],
      },
      {
        name: "Italy",
        isChecked: false,
        index: 2,
        isMapped: false,
        parent: null,
        children: [],
      },
      {
        name: "Russia",
        isChecked: false,
        index: 3,
        isMapped: false,
        parent: null,
        children: [],
      },
      {
        name: "Germany",
        isChecked: false,
        index: 4,
        isMapped: false,
        parent: null,
        children: [],
      },
    ],
  },

  {
    name: "District",
    values: [
      {
        name: "Madurai",
        isChecked: false,
        index: 0,
        isMapped: false,
        parent: null,
        children: [],
      },
      {
        name: "Coimbatore",
        isChecked: false,
        index: 1,
        isMapped: false,
        parent: null,
        children: [],
      },
      {
        name: "Chennai",
        isChecked: false,
        index: 2,
        isMapped: false,
        parent: null,
        children: [],
      },
      {
        name: "Tiruchirappalli",
        isChecked: false,
        index: 3,
        isMapped: false,
        parent: null,
        children: [],
      },
      {
        name: "Tiruppur",
        isChecked: false,
        index: 4,
        isMapped: false,
        parent: null,
        children: [],
      },
    ],
  },

  {
    name: "Town",
    values: [
      {
        name: "Tiruppur",
        index: 0,
        isMapped: false,
        isChecked: false,
        parent: null,
        children: [],
      },
      {
        name: "Tiruchirappalli",
        index: 1,
        isMapped: false,
        isChecked: false,
        parent: null,
        children: [],
      },
      {
        name: "Madurai",
        index: 2,
        isMapped: false,
        isChecked: false,
        parent: null,
        children: [],
      },
      {
        name: "Coimbatore",
        index: 3,
        isMapped: false,
        isChecked: false,
        parent: null,
        children: [],
      },
    ],
  },
];

let summaryContainerList: any[] = [
  {
    field: "",
    childrenGroupFieldName: "",
    childre: [],
  },
];

const DependencyMappingLayout = ({}) => {
  const [currentFieldIsParent, setCurrentFieldIsParent] = useState(false);
  const [isLoaded, setIsLoaded] = useState(false);
  const [parentMappingdDataSource, setParentMappingdDataSource] =
    useState(fieldValues);
  const [comboBoxDataSource, setcomboBoxDataSource] = useState(existingFields);
  const [fieldChildren, setFieldChildren] = useState(fieldChildrenGroupNames);
  const [summaryContainerDataSource, setSummaryContainerDataSource] =
    useState(summaryContainerList);
  const [childMappingDataSource, setChildMappingDataSource] =
    useState(fieldValues);
  const comboBoxRef = useRef(null);
  const [comboBoxSelectedFieldName, setComboBoxSelectedFieldName] =
    useState("");
  const [parentFieldName, setParentFieldName] = useState("Not Choosen");
  const [parentListViewSelectedIndex, setParentListViewSelectedIndex] =
    useState(-1);
  const [selectAllCheckBoxState, setSelectAllCheckBoxState] = useState(false);

  const addParent = () => {
    setIsLoaded(true);
    setCurrentFieldIsParent(false);
    setParentMappingdDataSource([]);
    setChildMappingDataSource(fieldValues);
  };

  const addChild = () => {
    setIsLoaded(true);
    setCurrentFieldIsParent(true);
    setParentMappingdDataSource(fieldValues);
    setChildMappingDataSource([]);
    const updatedChildren = [...fieldChildren];
    updatedChildren.push("Not Choosen");
    setFieldChildren(updatedChildren);
  };

  const onComboBoxChange = (args) => {
    if (args.value == null) return;

    setComboBoxSelectedFieldName(args.value);

    const values = existingFields.find(
      (field) => field.name === args.value
    ).values;

    if (currentFieldIsParent) {
      setChildMappingDataSource(values);
      const updatedFieldChildren = [...fieldChildren];
      updatedFieldChildren[updatedFieldChildren.length - 1] = args.value;
      setFieldChildren(updatedFieldChildren);
    } else {
      setParentMappingdDataSource(values);
      setParentFieldName(args.value);
    }
  };

  const onChildListViewResetAllClick = (args) => {
    const updatedChildMappingDataSource = [...childMappingDataSource];

    for (let i = 0; i < updatedChildMappingDataSource.length; i++) {
      updatedChildMappingDataSource[i].isChecked = false;
    }
    setChildMappingDataSource(updatedChildMappingDataSource);

    const parentListViewSelectedItem =
      parentMappingdDataSource[parentListViewSelectedIndex];
    parentListViewSelectedItem.children = [];
    parentListViewSelectedItem.isMapped = false;
    setSelectAllCheckBoxState(false);
  };

  const onChildListViewSelectAllClick = (args) => {
    const updatedChildMappingDataSource = [...childMappingDataSource];
    const parentListViewSelectedItem =
      parentMappingdDataSource[parentListViewSelectedIndex];

    for (let i = 0; i < updatedChildMappingDataSource.length; i++) {
      updatedChildMappingDataSource[i].isChecked = args.target.checked;
      if (args.target.checked)
        parentListViewSelectedItem.children.push(
          updatedChildMappingDataSource[i].name
        );
    }
    if (!args.target.checked) parentListViewSelectedItem.children = [];
    setChildMappingDataSource(updatedChildMappingDataSource);

    parentListViewSelectedItem.isMapped = args.target.checked;
    setSelectAllCheckBoxState(args.target.checked);
  };

  const onParentListViewSelect = (index: number, args) => {
    // Get the selected item from the ListView
    if (index >= 0) setParentListViewSelectedIndex(index);
    const selectedItem = parentMappingdDataSource[index];
    const updatedChildMappingDataSource = [...childMappingDataSource];
    for (let i = 0; i < updatedChildMappingDataSource.length; i++) {
      updatedChildMappingDataSource[i].isChecked = false;
    }
    if (selectedItem != null && selectedItem.children.length > 0) {
      for (let i = 0; i < selectedItem.children.length; i++) {
        const childItem = updatedChildMappingDataSource.find(
          (item) => item.name === selectedItem.children[i]
        );
        if (childItem != null) childItem.isChecked = true;
      }
    }
    setChildMappingDataSource(updatedChildMappingDataSource);
  };

  const handleCheckboxChange = (event, item) => {
    item.isChecked = event.target.checked;

    const parentListViewSelectedItem =
      parentMappingdDataSource[parentListViewSelectedIndex];

    if (parentListViewSelectedItem == null) return;

    if (item.isChecked) {
      parentListViewSelectedItem.children.push(item.name);
    } else
      parentListViewSelectedItem.children =
        parentListViewSelectedItem.children.filter(
          (id: string) => id !== item.name
        );

    if (parentListViewSelectedItem.children.length > 0) {
      parentListViewSelectedItem.isMapped = true;
      UpdateSummaryListViewUpdate(parentListViewSelectedItem);

      if (currentFieldIsParent) {
        const updatedChildren = [...fieldChildren]; // Create a copy of the fieldChildren array

        setFieldChildren(updatedChildren);
      }
    } else parentListViewSelectedItem.isMapped = false;
  };

  const UpdateSummaryListViewUpdate = (parentListViewSelectedItem) => {
    const updatedDataSource = [...summaryContainerDataSource];

    const existingItem = summaryContainerDataSource.find(
      (item) =>
        item.childrenGroupFieldName === comboBoxSelectedFieldName &&
        item.field === parentListViewSelectedItem.name
    );

    if (existingItem != null) {
      existingItem.children = parentListViewSelectedItem.children;
    } else {
      const newItem = {
        field: parentListViewSelectedItem.name,
        childrenGroupFieldName: comboBoxSelectedFieldName,
        children: parentListViewSelectedItem.children,
      };

      setSummaryContainerDataSource((list) => [...list, newItem]);
    }

    // setSummaryContainerDataSource([]);
    // for (let i = 0; i < parentMappingdDataSource.length; i++) {
    //   if (parentMappingdDataSource[i].isMapped) {
    //     const newItem = {
    //       field: parentListViewSelectedItem.name,
    //       childrenGroupFieldName: comboBoxSelectedFieldName,
    //       children: parentListViewSelectedItem.children,
    //     };
    //     setSummaryContainerDataSource((list) => [...list, newItem]);
    //   }
    // }
  };

  return (
    <div className={styles.dependencyMappingParentLayout}>
      <label className={styles.headerLabel}>Add Dependency</label>
      <hr className={styles.horizontalLine} />
      <MappingActionContainer
        addParent={addParent}
        parentFieldName={parentFieldName}
        fieldName={fieldName}
        addChild={addChild}
        fieldChildren={fieldChildren}
        currentFieldIsChild={!currentFieldIsParent}
        isLoaded={isLoaded}
      />
      {isLoaded && (
        <div className={styles.mappingGridContainer}>
          <ParentMappingContainer
            isParent={currentFieldIsParent}
            parentMappingdDataSource={parentMappingdDataSource}
            comboBoxRef={comboBoxRef}
            onComboBoxChange={onComboBoxChange}
            comboBoxDataSource={comboBoxDataSource}
            onParentListViewSelect={onParentListViewSelect}
            fieldName={fieldName}
          />
          <div className={styles.mappingIconContainer}>
            <Image
              src="/mapping_double_arrow_icon.png"
              className={styles.doubleArrowIcon}
              alt="Dependency Mapping Icon"
              height={25}
              width={25}
            />
          </div>
          <ChildMappingContainer
            isChild={!currentFieldIsParent}
            onComboBoxChange={onComboBoxChange}
            comboBoxDataSource={comboBoxDataSource}
            childMappingDataSource={childMappingDataSource}
            comboBoxRef={comboBoxRef}
            fieldName={fieldName}
            handleCheckboxChange={handleCheckboxChange}
            onChildListViewResetAllClick={onChildListViewResetAllClick}
            onChildListViewSelectAllClick={onChildListViewSelectAllClick}
            selectAllCheckBoxState={selectAllCheckBoxState}
          />
          <SummaryContainer
            summaryContainerDataSource={summaryContainerDataSource}
          />
        </div>
      )}
      {!isLoaded && <div className={styles.emptyContainer}></div>}
      <hr className={styles.horizontalLine} />
      <div className={styles.footerContainer}>
        <ButtonComponent>Cancel</ButtonComponent>
        <ButtonComponent className={styles.saveButton}>Save</ButtonComponent>
      </div>
    </div>
  );
};

export default DependencyMappingLayout;

function MappingActionContainer({
  addParent,
  parentFieldName,
  fieldName,
  addChild,
  fieldChildren,
  currentFieldIsChild,
  isLoaded,
}) {
  return (
    <div className={styles.mappingActionContainer}>
      <div>
        <div>
          <label className={styles.mappingActionLabel}>
            {fieldName} is child of :
          </label>
        </div>
        <div>
          {(!isLoaded || !currentFieldIsChild) &&
            parentFieldName == "Not Choosen" && (
              <ButtonComponent
                cssClass="e-link"
                className={styles.mappingActionAddParentButton}
                onClick={addParent}
              >
                +Add a Parent
              </ButtonComponent>
            )}
          {((isLoaded && currentFieldIsChild) ||
            parentFieldName != "Not Choosen") && (
            <ButtonComponent className={styles.parentFieldButton}>
              {parentFieldName}
            </ButtonComponent>
          )}
        </div>
      </div>
      <div>
        <div>
          <label className={styles.mappingActionLabel}>
            {fieldName} is parent of :
          </label>
        </div>
        <div>
          <>
            {fieldChildren.map((child, index) => (
              <ButtonComponent
                className={styles.childFieldButton}
                key={index}
                style={{
                  borderRadius:
                    index === 0
                      ? fieldChildren.length === 1
                        ? "20px"
                        : "20px 0px 0px 20px"
                      : index === fieldChildren.length - 1
                      ? "0px 20px 20px 0px"
                      : "0px",
                  marginRight: index === fieldChildren.length - 1 ? "10px" : 0,
                }}
              >
                Child {index + 1}: {child}
              </ButtonComponent>
            ))}
          </>
          {fieldChildren.length < 4 && (
            <ButtonComponent
              cssClass="e-link"
              className={styles.mappingActionAddChildButton}
              onClick={addChild}
            >
              +Add a Child
            </ButtonComponent>
          )}
        </div>
      </div>
    </div>
  );
}

function ParentMappingContainer({
  isParent,
  comboBoxRef,
  onComboBoxChange,
  parentMappingdDataSource,
  comboBoxDataSource,
  onParentListViewSelect,
  fieldName,
}) {
  return (
    <div className={styles.parentMappingConatiner}>
      <MappingHeaderComponent
        headerLabel={
          !isParent ? "Choose Parent Field/Group" : "Parent Field/Group"
        }
        isChild={isParent}
        onComboBoxChange={onComboBoxChange}
        comboBoxRef={comboBoxRef}
        comboBoxDataSource={comboBoxDataSource}
        fieldName={fieldName}
        isChildContainer={false}
        onChildListViewResetAllClick={() => {}}
      />
      <ul>
        {parentMappingdDataSource.map((item, index) => (
          <>{parentMappingListViewItemTemplate(item, onParentListViewSelect)}</>
        ))}
      </ul>
    </div>
  );
}

function ChildMappingContainer({
  isChild,
  onComboBoxChange,
  comboBoxRef,
  comboBoxDataSource,
  childMappingDataSource,
  fieldName,
  handleCheckboxChange,
  onChildListViewResetAllClick,
  onChildListViewSelectAllClick,
  selectAllCheckBoxState,
}) {
  return (
    <div className={styles.childMappingContainer}>
      <MappingHeaderComponent
        headerLabel={
          !isChild ? "Choose Child Field/Group" : "Child Field/Group"
        }
        isChild={isChild}
        onComboBoxChange={onComboBoxChange}
        comboBoxRef={comboBoxRef}
        comboBoxDataSource={comboBoxDataSource}
        fieldName={fieldName}
        isChildContainer={true}
        onChildListViewResetAllClick={onChildListViewResetAllClick}
      />
      {childMappingDataSource.length > 0 && (
        <div className={styles.childMappingListViewItem}>
          <CheckBoxComponent
            type="checkbox"
            checked={selectAllCheckBoxState}
            onChange={(event) => onChildListViewSelectAllClick(event)}
          />
          <label className={styles.childMappingListViewItemLabel}>
            Select All
          </label>
        </div>
      )}
      <ul>
        {childMappingDataSource.map((data, index) => (
          <React.Fragment key={index}>
            {childMappingListViewItemTemplate(data, handleCheckboxChange)}
          </React.Fragment>
        ))}
      </ul>
    </div>
  );
}

function SummaryContainer({ summaryContainerDataSource }) {
  return (
    <div className={styles.summaryContainer}>
      <label className={styles.summaryContainerHeader}>Summary</label>
      <ListViewComponent
        dataSource={summaryContainerDataSource}
        template={summaryListItemTemplate}
      />
    </div>
  );
}

function MappingHeaderComponent({
  headerLabel,
  isChild,
  onComboBoxChange,
  comboBoxRef,
  comboBoxDataSource,
  fieldName,
  isChildContainer,
  onChildListViewResetAllClick,
}) {
  return (
    <div>
      <div
        className={styles.mappingHeaderContainer}
        style={{ backgroundColor: isChild ? "#F2F4F7" : "#F9F5FF" }}
      >
        <div style={{ display: "flex", justifyContent: "space-between" }}>
          <label className={styles.mappingHeaderLabel}>{headerLabel}</label>
          {isChildContainer && (
            <ButtonComponent
              cssClass="e-link"
              className={styles.resetAllButton}
              onClick={onChildListViewResetAllClick}
            >
              Reset All
            </ButtonComponent>
          )}
        </div>
        {isChild && (
          <div className={styles.mappingHeaderRoundedContainer}>
            <Image
              src="/field_icon.png"
              className={styles.mappingHeaderFieldIcon}
              alt="Field Icon"
              height={15}
              width={13}
            />
            <label className={styles.mappingHeaderFieldLabel}>
              {fieldName}
            </label>
          </div>
        )}
        {!isChild && (
          <ComboBoxComponent
            cssClass={styles.mappingHeaderComboBox}
            dataSource={comboBoxDataSource}
            ref={comboBoxRef}
            fields={{ text: "name", value: "name" }}
            onChange={onComboBoxChange}
            valueTemplate={comboBoxValueTemplate}
            placeholder="Select a Field"
          />
        )}
      </div>
      <DependencyMappingSearchBoxComponent />
    </div>
  );
}

function DependencyMappingSearchBoxComponent() {
  return (
    <TextBoxComponent
      type="text"
      cssClass={styles.mappingSearchTextBox}
      placeholder=" Search"
      title="Type in a name"
    />
  );
}

function parentMappingListViewItemTemplate(data, onParentListViewSelect) {
  return (
    <li className={styles.parentMappingListViewItemRoot}>
      <div
        className={styles.parentMappingListViewItem}
        onClick={onParentListViewSelect.bind(null, data.index)}
      >
        <label className={styles.parentMappingListViewItemLabel}>
          {data.name}
        </label>
        {data.isMapped && (
          <label className={styles.parentMappingItemRoundedLabel}>Mapped</label>
        )}
      </div>
    </li>
  );
}

function childMappingListViewItemTemplate(data, handleCheckboxChange) {
  return (
    <li>
      <div className={styles.childMappingListViewItem}>
        <CheckBoxComponent
          type="checkbox"
          checked={data.isChecked}
          onChange={(event) => handleCheckboxChange(event, data)}
        />
        <label className={styles.childMappingListViewItemLabel}>
          {data.name}
        </label>
      </div>
    </li>
  );
}

function comboBoxValueTemplate(data) {
  return (
    <div className={styles.comboBoxValueTemplateContainer}>
      <Image
        src="/field_icon.png"
        className={styles.mappingHeaderFieldIcon}
        alt="Field Icon"
        height={15}
        width={13}
      />
      <label className={styles.mappingHeaderFieldLabel}>{data.name}</label>
    </div>
  );
}

function summaryListItemTemplate(data) {
  return (
    <div className={styles.summaryListItemRootContainer}>
      <label className={styles.summaryListItemHeader}>{data.field} : </label>

      <ListViewComponent
        id="list"
        dataSource={data.children}
      ></ListViewComponent>
      {/* <ul className={styles.summaryListItemContainer}>
        {data.children.map((item, index) => (
          <React.Fragment key={item}>
            <label>{item}</label>
            {index !== data.children.length - 1 && (
              <span className={styles.comma}>, </span>
            )}
          </React.Fragment>
        ))}
      </ul> */}
    </div>
  );
}
