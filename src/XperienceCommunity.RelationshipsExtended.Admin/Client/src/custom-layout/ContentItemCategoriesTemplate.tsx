import React, { useState } from "react";
import { Box, Button, ButtonColor, Checkbox, CheckboxSize, Headline, HeadlineSize, Icon, Input, Paper, Spacing } from "@kentico/xperience-admin-components";
import { usePageCommand } from "@kentico/xperience-admin-base";

/*
* This file demonstrates a custom UI page template.
  The template supports a single page command that retrieves a string value from the backend.

  In this example, the command retrieves the server's DateTime.Now value and displays it in a label.
  See ~\UIPages\CustomTemplate\CustomTemplate.cs for the backend definition of the page.
*/

interface TaxonomyItem {
  readonly taxonomyName: string;
  readonly taxonomyDisplayName: string;
  readonly categories: Array<TaxonomyCategoryItem>;
}
interface TaxonomyCategoryItem {
  readonly categoryName: string;
  readonly categoryDisplayName: string;
}

interface CategoryItemCategoryProps {
  readonly enabled: boolean;
  readonly selectedCategories: Array<string>;
  readonly availableCategories: Array<TaxonomyItem>;
}

interface SetCategoriesResult {
  readonly selectedCategories: Array<string>;
}

interface SetCategoriesArguments {
  readonly selectedCategories: Array<string>;
}

const Commands = {
  SetCategories: "SetCategories",
};

export const ContentItemCategoriesTemplate = ({ enabled, selectedCategories, availableCategories }: CategoryItemCategoryProps) => {
  const [selectedCategoryNames, setSelectedCategoryNames] = useState(selectedCategories);
  const [filterValue, setFilterValue] = useState("");

  const { execute: submit } = usePageCommand<SetCategoriesResult, SetCategoriesArguments>(
    Commands.SetCategories,
    {
      data:  { selectedCategories: selectedCategoryNames},
      after: (response) => {
        if(response) {
          setSelectedCategoryNames(response.selectedCategories);
        }
      },
    }
  );

  var toggleCategoryCheck = function (event : React.ChangeEvent<HTMLInputElement>, checked: Boolean) : void {
    if(checked) {
      setSelectedCategoryNames([...selectedCategoryNames, event.target.name.toLowerCase()])
    } else {
      setSelectedCategoryNames(selectedCategoryNames.filter(function(item) {
        return item.toLowerCase() != event.target.name.toLowerCase();
      }));
    }
  }

  return (
    <div>
      <Paper fullHeight>
      <Box spacing={Spacing.XXL} >

      {enabled &&
        <>
          <Headline size={HeadlineSize.L} spacingTop={Spacing.Micro} spacingBottom={Spacing.S}>Categories</Headline>
          <Box>These categories are shared amongst all language variations of this Content Item and are not part of any workflow.  <br/><br/>Changes will take affect immediately and impact all language variations.</Box>
          
          <div style={{paddingTop: 15}}>
          <Input actionElement={
            <>
            <div onClick={() => setFilterValue('')} title='Clear Filter' style={{marginRight: 13, color: 'black', cursor: 'pointer'}}>
              <Icon name={'xp-x'} />
            </div>
          </>} label="Filter" type="text" tooltipText="Filter Categories Visible" value={filterValue} onChange={(e) => setFilterValue(e.target.value)} />
          </div>
          <ul style={{paddingLeft: 0, marginLeft: 0, color: "black", listStyle: "none"}}>
          {availableCategories.map((element, i) => {
            var filteredItems = element.categories.filter((category, i) => {
              return filterValue.length === 0 || category.categoryDisplayName.toLowerCase().indexOf(filterValue.toLowerCase()) > -1;
            });
            return(
              <>
              {filteredItems.length > 0 &&
                <li key={element.taxonomyName}>
                  <Headline size={HeadlineSize.M} >{element.taxonomyDisplayName}</Headline>
                  <ul style={{color: "black", listStyle: "none"}}>
                    {element.categories.filter((category, i) => {
                      return filterValue.length === 0 || category.categoryDisplayName.toLowerCase().indexOf(filterValue.toLowerCase()) > -1;
                    }).map((category, i) => {
                      return(
                      <li key={category.categoryName}>
                        <Checkbox label={category.categoryDisplayName} size={CheckboxSize.L} name={category.categoryName} checked={(selectedCategoryNames.indexOf(category.categoryName) > -1)} onChange={toggleCategoryCheck} />
                      </li> 
                      )
                    })}
                  </ul>
                </li>
              }
              </>
            )}
          )}
          </ul>
          <Button onClick={() => submit()} color={ButtonColor.Primary} label="Save Changes"  />
        </>
      }
      </Box>
      </Paper>
    </div>
  );
};
