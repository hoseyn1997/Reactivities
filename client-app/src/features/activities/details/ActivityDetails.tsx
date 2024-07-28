import { Grid } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import LoadingComponents from "../../../app/layout/LoadingComponent";
import { useEffect } from "react";
import { useParams } from "react-router-dom";
import { observer } from "mobx-react-lite";
import ActivityDetailedInfo from "./ActivityDetailedInfo";
import ActivityDetailedChat from "./ActivityDetailedChat";
import ActivityDetailedSidebar from "./ActivityDetailedSidebar";
import ActivityDetailedHeader from "./ActivityDetailedHeader";

export default observer(function ActivityDetails() {
  const { activityStore } = useStore();
  const { id } = useParams();
  const { selectedActivity: activity, clearSelectedActivity } = activityStore;

  useEffect(() => {
    if (id) activityStore.loadActivity(id);
    return () => clearSelectedActivity();
  }, [id, activityStore, clearSelectedActivity]);

  if (!activity) return <LoadingComponents content={""} />;
  return (
    <Grid>
      <Grid.Column width="10">
        <ActivityDetailedHeader activity={activity} />
        <ActivityDetailedInfo activity={activity} />
        <ActivityDetailedChat activityId={activity.id} />
      </Grid.Column>
      <Grid.Column width="6">
        <ActivityDetailedSidebar activity={activity} />
      </Grid.Column>
    </Grid>
  );
});
