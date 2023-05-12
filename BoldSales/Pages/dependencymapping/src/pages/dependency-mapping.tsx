import styles from "@/styles/DependencyMapping.module.css";
import { ButtonComponent } from "@syncfusion/ej2-react-buttons";
import { ComboBoxComponent } from "@syncfusion/ej2-react-dropdowns";
import { TextBoxComponent } from "@syncfusion/ej2-react-inputs";
import { CheckBoxComponent } from "@syncfusion/ej2-react-buttons";
import { DataManager, Query } from "@syncfusion/ej2-data";
import { MouseEventHandler, useState } from "react";
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

let summaryContainerList: any[] = [];

const DependencyMappingLayout = ({}) => {
  const [parentListViewDataSource, setParentListViewDataSource] = useState({
    parentListData: fieldValues,
  });
  const [summaryContainerDataSource, setSummaryContainerDataSource] = useState({
    summaryListData: summaryContainerList,
  });
  const [childListViewDataSource, setChildListViewDataSource] = useState({
    childListData: fieldValues,
  });
  const [comboBoxDataSource, setcomboBoxDataSource] = useState(existingFields);
  const [fieldChildren, setFieldChildren] = useState(fieldChildrenGroupNames);
  const [comboBoxSelectedFieldName, setComboBoxSelectedFieldName] =
    useState("");
  const [parentFieldName, setParentFieldName] = useState("Not Choosen");
  const [parentListViewSelectedIndex, setParentListViewSelectedIndex] =
    useState(-1);
  const [selectAllCheckBoxState, setSelectAllCheckBoxState] = useState(false);
  const [currentFieldIsParent, setCurrentFieldIsParent] = useState(false);
  const [isLoaded, setIsLoaded] = useState(false);

  const addParent = () => {
    setIsLoaded(true);
    setCurrentFieldIsParent(false);
    setParentListViewDataSource({ parentListData: [] });
    setSummaryContainerDataSource({ summaryListData: [] });
    setChildListViewDataSource({ childListData: fieldValues });
  };

  const addChild = () => {
    setIsLoaded(true);

    if (!currentFieldIsParent)
      setSummaryContainerDataSource({ summaryListData: [] });

    setCurrentFieldIsParent(true);
    setParentListViewDataSource({ parentListData: fieldValues });
    setChildListViewDataSource({ childListData: [] });
    const updatedChildren = [...fieldChildren];
    updatedChildren.push("Not Choosen");
    setFieldChildren(updatedChildren);
  };

  const onComboBoxChange = (args: any) => {
    if (args.value == null) return;

    setComboBoxSelectedFieldName(args.value);

    const values = existingFields.find(
      (field) => field.name === args.value
    ).values;

    if (currentFieldIsParent) {
      setChildListViewDataSource({ childListData: values });
      const updatedFieldChildren = [...fieldChildren];
      updatedFieldChildren[updatedFieldChildren.length - 1] = args.value;
      setFieldChildren(updatedFieldChildren);
    } else {
      setParentListViewDataSource({ parentListData: values });
      setParentFieldName(args.value);
    }
  };

  const onChildListViewResetAllClick = (args: MouseEventHandler) => {
    UpdateSummaryListDataSource(false);
  };

  const onChildListViewSelectAllClick = (args: any) => {
    UpdateSummaryListDataSource(args.target.checked);
  };

  const onParentListViewSelect = (index: number, args: MouseEventHandler) => {
    // Get the selected item from the ListView
    if (index >= 0) setParentListViewSelectedIndex(index);
    const selectedItem = parentListViewDataSource.parentListData[index];
    const updatedChildMappingDataSource = [
      ...childListViewDataSource.childListData,
    ];
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
    setChildListViewDataSource({
      childListData: updatedChildMappingDataSource,
    });
  };

  const handleCheckboxChange = (event: any, item: any) => {
    item.isChecked = event.target.checked;

    if (parentListViewSelectedIndex < 0) return;

    const parentListViewSelectedItem =
      parentListViewDataSource.parentListData[parentListViewSelectedIndex];

    if (item.isChecked) {
      parentListViewSelectedItem.children.push(item.name);
    } else
      parentListViewSelectedItem.children =
        parentListViewSelectedItem.children.filter(
          (id: string) => id !== item.name
        );

    if (parentListViewSelectedItem.children.length > 0) {
      parentListViewSelectedItem.isMapped = true;
    } else parentListViewSelectedItem.isMapped = false;

    UpdateSummaryListViewUpdate(parentListViewSelectedItem);
  };

  function handleSearch(event: any, isChildContainer: boolean) {
    let value = event.value;

    if (isChildContainer) {
      let data = new DataManager(
        childListViewDataSource.childListData
      ).executeLocal(new Query().where("name", "startswith", value, true));
      if (!value) {
        setChildListViewDataSource({
          childListData: fieldValues,
        });
      } else {
        setChildListViewDataSource({
          childListData: data,
        });
      }
    }
  }

  const UpdateSummaryListViewUpdate = (parentListViewSelectedItem: any) => {
    //TODO : Need to revamp this.
    const updatedList = summaryContainerDataSource.summaryListData.filter(
      (item) =>
        !(
          item.childrenGroupFieldName === comboBoxSelectedFieldName &&
          item.field === parentListViewSelectedItem.name
        )
    );

    setSummaryContainerDataSource((prevState) => ({
      ...prevState,
      summaryListData: updatedList,
    }));

    const newItem = {
      field: parentListViewSelectedItem.name,
      childrenGroupFieldName: comboBoxSelectedFieldName,
      children: parentListViewSelectedItem.children,
    };

    setSummaryContainerDataSource((prevState) => {
      return {
        ...prevState,
        summaryListData: [...prevState.summaryListData, newItem],
      };
    });
  };

  const UpdateSummaryListDataSource = (isSelectAll: boolean) => {
    if (parentListViewSelectedIndex < 0) return;

    const updatedChildMappingDataSource = [
      ...childListViewDataSource.childListData,
    ];
    const parentListViewSelectedItem =
      parentListViewDataSource.parentListData[parentListViewSelectedIndex];

    for (let i = 0; i < updatedChildMappingDataSource.length; i++) {
      updatedChildMappingDataSource[i].isChecked = isSelectAll;
      if (isSelectAll)
        parentListViewSelectedItem.children.push(
          updatedChildMappingDataSource[i].name
        );
    }
    if (!isSelectAll) parentListViewSelectedItem.children = [];
    setChildListViewDataSource({
      childListData: updatedChildMappingDataSource,
    });

    parentListViewSelectedItem.isMapped = isSelectAll;
    setSelectAllCheckBoxState(isSelectAll);

    UpdateSummaryListViewUpdate(parentListViewSelectedItem);
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
            parentListViewDataSource={parentListViewDataSource.parentListData}
            onComboBoxChange={onComboBoxChange}
            comboBoxDataSource={comboBoxDataSource}
            onParentListViewSelect={onParentListViewSelect}
            fieldName={fieldName}
            handleSearch={handleSearch}
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
            childListViewDataSource={childListViewDataSource.childListData}
            fieldName={fieldName}
            handleCheckboxChange={handleCheckboxChange}
            onChildListViewResetAllClick={onChildListViewResetAllClick}
            onChildListViewSelectAllClick={onChildListViewSelectAllClick}
            selectAllCheckBoxState={selectAllCheckBoxState}
            handleSearch={handleSearch}
          />
          <SummaryContainer
            summaryContainerDataSource={
              summaryContainerDataSource.summaryListData
            }
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
}: {
  addParent: MouseEventHandler;
  parentFieldName: string;
  fieldName: string;
  addChild: MouseEventHandler;
  fieldChildren: string[];
  currentFieldIsChild: boolean;
  isLoaded: boolean;
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
                className={styles.mappingActionAddButton}
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
              className={styles.mappingActionAddButton}
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
  onComboBoxChange,
  parentListViewDataSource,
  comboBoxDataSource,
  onParentListViewSelect,
  fieldName,
  handleSearch,
}: {
  isParent: boolean;
  onComboBoxChange: any;
  parentListViewDataSource: string[];
  comboBoxDataSource: string[];
  onParentListViewSelect: any;
  fieldName: string;
  handleSearch: any;
}) {
  const [selectedItem, setSelectedItem] = useState(null);
  const handleItemClick = (item: any) => {
    setSelectedItem(item);
  };
  return (
    <div className={styles.parentMappingConatiner}>
      <MappingHeaderComponent
        headerLabel={
          !isParent ? "Choose Parent Field/Group" : "Parent Field/Group"
        }
        isChild={isParent}
        onComboBoxChange={onComboBoxChange}
        comboBoxDataSource={comboBoxDataSource}
        fieldName={fieldName}
        isChildContainer={false}
        onChildListViewResetAllClick={() => {}}
        handleSearch={handleSearch}
      />
      <ul>
        {parentListViewDataSource.map((item: any) => (
          <li
            key={item}
            onClick={() => handleItemClick(item)}
            className={`${styles.parentMappingListViewRootItem} ${
              selectedItem === item
                ? styles.selectedParentMappingListViewItem
                : ""
            }`}
          >
            <div
              className={styles.parentMappingListViewItem}
              onClick={onParentListViewSelect.bind(null, item.index)}
            >
              <label className={styles.parentMappingListViewItemLabel}>
                {item.name}
              </label>
              {item.isMapped && (
                <label
                  className={`${styles.parentMappingItemRoundedLabel} ${
                    selectedItem === item
                      ? styles.selectedParentMappingItemRoundedLabel
                      : ""
                  }`}
                >
                  Mapped
                </label>
              )}
            </div>
          </li>
        ))}
      </ul>
    </div>
  );
}

function ChildMappingContainer({
  isChild,
  onComboBoxChange,
  comboBoxDataSource,
  childListViewDataSource,
  fieldName,
  handleCheckboxChange,
  onChildListViewResetAllClick,
  onChildListViewSelectAllClick,
  selectAllCheckBoxState,
  handleSearch,
}: {
  isChild: boolean;
  onComboBoxChange: any;
  comboBoxDataSource: string[];
  childListViewDataSource: any[];
  fieldName: string;
  handleCheckboxChange: any;
  onChildListViewResetAllClick: any;
  onChildListViewSelectAllClick: any;
  selectAllCheckBoxState: boolean;
  handleSearch: any;
}) {
  return (
    <div className={styles.childMappingContainer}>
      <MappingHeaderComponent
        headerLabel={
          !isChild ? "Choose Child Field/Group" : "Child Field/Group"
        }
        isChild={isChild}
        onComboBoxChange={onComboBoxChange}
        comboBoxDataSource={comboBoxDataSource}
        fieldName={fieldName}
        isChildContainer={true}
        onChildListViewResetAllClick={onChildListViewResetAllClick}
        handleSearch={handleSearch}
      />
      {childListViewDataSource.length > 0 && (
        <div className={styles.childMappingListViewItem}>
          <CheckBoxComponent
            type="checkbox"
            checked={selectAllCheckBoxState}
            onChange={(event: any) => onChildListViewSelectAllClick(event)}
          />
          <label className={styles.childMappingListViewItemLabel}>
            Select All
          </label>
        </div>
      )}
      <ul>
        {childListViewDataSource.map((data, index) => (
          <React.Fragment key={index}>
            {childMappingListViewItemTemplate(data, handleCheckboxChange)}
          </React.Fragment>
        ))}
      </ul>
    </div>
  );
}

function SummaryContainer({
  summaryContainerDataSource,
}: {
  summaryContainerDataSource: any[];
}) {
  return (
    <div className={styles.summaryContainer}>
      <label className={styles.summaryListItemHeader}>Summary</label>
      <ul>
        {summaryContainerDataSource.map((item) => (
          <li>
            <label className={styles.summaryListItemHeader}>
              {item.field} :{" "}
            </label>

            <ul className={styles.summaryListItemContainer}>
              {item.children.map((data: any, index: number) => (
                <>
                  <label className={styles.summaryListItemLabel}>{data}</label>
                  {index !== item.children.length - 1 && (
                    <span className={styles.comma}>, </span>
                  )}
                </>
              ))}
            </ul>
          </li>
        ))}
      </ul>
    </div>
  );
}

function MappingHeaderComponent({
  headerLabel,
  isChild,
  onComboBoxChange,
  comboBoxDataSource,
  fieldName,
  isChildContainer,
  onChildListViewResetAllClick,
  handleSearch,
}: {
  headerLabel: string;
  isChild: boolean;
  onComboBoxChange: any;
  comboBoxDataSource: string[];
  fieldName: string;
  isChildContainer: boolean;
  onChildListViewResetAllClick: any;
  handleSearch: any;
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
            fields={{ text: "name", value: "name" }}
            onChange={onComboBoxChange}
            valueTemplate={comboBoxValueTemplate}
            placeholder="Select a Field"
          />
        )}
      </div>
      <DependencyMappingSearchBoxComponent
        handleSearch={handleSearch}
        isChildContainer={isChildContainer}
      />
    </div>
  );
}

function DependencyMappingSearchBoxComponent({
  handleSearch,
  isChildContainer,
}: {
  handleSearch: any;
  isChildContainer: boolean;
}) {
  return (
    <TextBoxComponent
      type="text"
      cssClass={styles.mappingSearchTextBox}
      placeholder="Search"
      onChange={(event: any) => handleSearch(event, isChildContainer)}
    />
  );
}

function childMappingListViewItemTemplate(
  data: any,
  handleCheckboxChange: any
) {
  return (
    <li>
      <div className={styles.childMappingListViewItem}>
        <CheckBoxComponent
          type="checkbox"
          checked={data.isChecked}
          onChange={(event: any) => handleCheckboxChange(event, data)}
        />
        <label className={styles.childMappingListViewItemLabel}>
          {data.name}
        </label>
      </div>
    </li>
  );
}

function comboBoxValueTemplate(data: any) {
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
