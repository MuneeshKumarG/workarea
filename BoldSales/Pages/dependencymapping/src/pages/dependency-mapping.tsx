import styles from "@/styles/DependencyMapping.module.css";
import { ButtonComponent } from "@syncfusion/ej2-react-buttons";
import { ListViewComponent } from "@syncfusion/ej2-react-lists";
import { ComboBoxComponent } from "@syncfusion/ej2-react-dropdowns";
import { TextBoxComponent } from "@syncfusion/ej2-react-inputs";
import { useState } from "react";
import { useRef } from "react";
import Image from "next/image";

let fieldName = "State";
let fieldValues = [
  {
    isMapped: false,
    name: "TamilNadu",
    parent: null,
    children: { groupName: null, ids: [] },
  },
  {
    isMapped: false,
    name: "AndhraPradesh",
    parent: null,
    children: { groupName: null, ids: [] },
  },
  {
    isMapped: false,
    name: "Kerala",
    parent: null,
    children: { groupName: null, ids: [] },
  },
  {
    isMapped: false,
    name: "Karnataka",
    parent: null,
    children: { groupName: null, ids: [] },
  },
  {
    isMapped: false,
    name: "Telangana",
    parent: null,
    children: { groupName: null, ids: [] },
  },
];

let existingFields = [
  {
    name: "Country",
    values: [
      {
        name: "India",
        id: 1,
        isMapped: true,
        parent: null,
        children: { groupName: null, ids: [] },
      },
      {
        name: "USA",
        id: 2,
        isMapped: false,
        parent: null,
        children: { groupName: null, ids: [] },
      },
      {
        name: "Italy",
        id: 3,
        isMapped: false,
        parent: null,
        children: { groupName: null, ids: [] },
      },
      {
        name: "Russia",
        id: 4,
        isMapped: false,
        parent: null,
        children: { groupName: null, ids: [] },
      },
      {
        name: "Germany",
        id: 5,
        isMapped: true,
        parent: null,
        children: { groupName: null, ids: [] },
      },
    ],
  },

  {
    name: "District",
    values: [
      {
        name: "Madurai",
        id: 1,
        isMapped: false,
        parent: null,
        children: { groupName: null, ids: [] },
      },
      {
        name: "Coimbatore",
        id: 2,
        isMapped: true,
        parent: null,
        children: { groupName: null, ids: [] },
      },
      {
        name: "Chennai",
        id: 3,
        isMapped: false,
        parent: null,
        children: { groupName: null, ids: [] },
      },
      {
        name: "Tiruchirappalli",
        id: 4,
        isMapped: true,
        parent: null,
        children: { groupName: null, ids: [] },
      },
      {
        name: "Tiruppur",
        id: 5,
        isMapped: true,
        parent: null,
        children: { groupName: null, ids: [] },
      },
    ],
  },

  {
    name: "Town",
    values: [
      {
        name: "Tiruppur",
        id: 1,
        isMapped: true,
        parent: null,
        children: { groupName: null, ids: [] },
      },
      {
        name: "Tiruchirappalli",
        id: 3,
        isMapped: true,
        parent: null,
        children: { groupName: null, ids: [] },
      },
      {
        name: "Madurai",
        id: 4,
        isMapped: false,
        parent: null,
        children: { groupName: null, ids: [] },
      },
      {
        name: "Coimbatore",
        id: 2,
        isMapped: true,
        parent: null,
        children: { groupName: null, ids: [] },
      },
    ],
  },
];

const DependencyMappingLayout = ({}) => {
  const [isChild, setIsChild] = useState(false);
  const [isParent, setIsParent] = useState(true);
  const [parentMappingdDataSource, setParentMappingdDataSource] =
    useState(fieldValues);
  const [comboBoxDataSource, setcomboBoxDataSource] = useState(existingFields);
  const [childMappingDataSource, setChildMappingDataSource] = useState([]);
  const comboBoxRef = useRef(null);
  const [parentListSelectedItem, setParentListSelectedItem] = useState(null);

  const addParent = () => {
    setIsChild(true);
    setIsParent(false);
    setParentMappingdDataSource([]);
    setChildMappingDataSource(fieldValues);
    comboBoxRef.current.clear();
  };

  const addChild = () => {
    setIsChild(false);
    setIsParent(true);
    setParentMappingdDataSource(fieldValues);
    setChildMappingDataSource([]);
    comboBoxRef.current.clear();
  };

  const onChange = (args) => {
    const values = existingFields.find(
      (field) => field.name === args.value
    ).values;

    if (isParent) {
      setChildMappingDataSource(values);
    } else {
      setParentMappingdDataSource(values);
    }
  };

  const onParentListViewSelect = (args) => {
    // Get the selected item from the ListView
    const selectedItems = args.selectedItems;
    if (selectedItems && selectedItems.length > 0) {
      setParentListSelectedItem(selectedItems[0]);
    } else {
      setParentListSelectedItem(null);
    }
  };

  return (
    <div>
      <label className={styles.headerLabel}>
        Map the controlled Fields and dependent field
      </label>
      <ButtonComponent className={styles.resetAllButton}>
        Reset All
      </ButtonComponent>
      <div className={styles.mappingGridContainer}>
        <MappingActionContainer
          fieldName={fieldName}
          addParent={addParent}
          addChild={addChild}
        />
        <div className={styles.verticalLine}></div>
        <ParentMappingContainer
          isParent={isParent}
          parentMappingdDataSource={parentMappingdDataSource}
          comboBoxRef={comboBoxRef}
          onChange={onChange}
          comboBoxDataSource={comboBoxDataSource}
          onParentListViewSelect={onParentListViewSelect}
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
          isChild={isChild}
          onChange={onChange}
          comboBoxDataSource={comboBoxDataSource}
          childMappingDataSource={childMappingDataSource}
          comboBoxRef={comboBoxRef}
        />
        <SummaryContainer />
      </div>
      <hr className={styles.horizontalLine} />
      <div className={styles.footerContainer}>
        <ButtonComponent>Cancel</ButtonComponent>
        <ButtonComponent className={styles.saveButton}>Save</ButtonComponent>
      </div>
    </div>
  );
};

export default DependencyMappingLayout;

function MappingActionContainer({ fieldName, addParent, addChild }) {
  return (
    <div className={styles.mappingActionContainer}>
      <label className={styles.mappingActionLabel}>
        {fieldName} is a child of:
      </label>
      <ButtonComponent
        className={styles.mappingActionButton}
        onClick={addParent}
      >
        + Add a Parent
      </ButtonComponent>
      <label className={styles.mappingActionLabel}>
        {fieldName} is a parent of:
      </label>
      <ButtonComponent
        className={styles.mappingActionButton}
        onClick={addChild}
      >
        + Add a Child (Field Dependency)
      </ButtonComponent>
    </div>
  );
}

function ParentMappingContainer({
  isParent,
  comboBoxRef,
  onChange,
  parentMappingdDataSource,
  comboBoxDataSource,
  onParentListViewSelect,
}) {
  return (
    <div className={styles.parentMappingConatiner}>
      <MappingHeaderComponent
        headerLabel="Parent"
        isChild={isParent}
        onChange={onChange}
        comboBoxRef={comboBoxRef}
        comboBoxDataSource={comboBoxDataSource}
      />

      <ListViewComponent
        id="list"
        select={onParentListViewSelect}
        template={parentMappingListViewItemTemplate}
        dataSource={parentMappingdDataSource}
      />
    </div>
  );
}

function ChildMappingContainer({
  isChild,
  onChange,
  comboBoxRef,
  comboBoxDataSource,
  childMappingDataSource,
}) {
  return (
    <div className={styles.childMappingContainer}>
      <MappingHeaderComponent
        headerLabel="Child"
        isChild={isChild}
        onChange={onChange}
        comboBoxRef={comboBoxRef}
        comboBoxDataSource={comboBoxDataSource}
      />
      <ListViewComponent
        dataSource={childMappingDataSource}
        template={childMappingListViewItemTemplate}
        cssClass="e-list-template"
      />
    </div>
  );
}

function SummaryContainer() {
  return (
    <div className={styles.summaryContainer}>
      <div>
        <label className={styles.summaryContainerHeader}>Summary</label>
      </div>
    </div>
  );
}

function MappingHeaderComponent({
  headerLabel,
  isChild,
  onChange,
  comboBoxRef,
  comboBoxDataSource,
}) {
  return (
    <div>
      <div className={styles.mappingHeaderContainer}>
        <label className={styles.mappingHeaderLabel}>
          {headerLabel} field/Group
        </label>
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
            onChange={onChange}
            valueTemplate={comboBoxValueTemplate}
            placeholder="Select a Field"
          />
        )}
      </div>
      <DependencyMappingSearchBoxComponent />
      <hr className={styles.mappingHeaderHorizontalLine} />
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

function parentMappingListViewItemTemplate(data) {
  return (
    <div className={styles.parentMappingListViewItem}>
      <label className={styles.parentMappingListViewItemLabel}>
        {data.name}
      </label>
      {data.isMapped && (
        <label className={styles.parentMappingItemRoundedLabel}>Mapped</label>
      )}
    </div>
  );
}

function childMappingListViewItemTemplate(data) {
  return (
    <div className={styles.childMappingListViewItem}>
      <input type="checkbox" />
      <label className={styles.childMappingListViewItemLabel}>
        {data.name}
      </label>
    </div>
  );
}

function comboBoxValueTemplate(data) {
  return (
    <div className={styles.mappingIconContainer}>
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
