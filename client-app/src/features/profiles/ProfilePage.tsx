import { Button, Divider, Header } from "semantic-ui-react";
import { observer, useStaticRendering } from "mobx-react-lite";
import { useParams } from "react-router-dom";
import { useStore } from "../../app/stores/store";
import { useEffect, useState } from "react";
import LoadingComponents from "../../app/layout/LoadingComponent";
import TestComments from "../testComment/TestComments";

export default observer(function ProfilePage() {
  const [TalkTo, setTalkTo] = useState<string | null>(null);

  const { username } = useParams<{ username: string }>();
  const { profileStore, commentStore, userStore, modalStore } = useStore();
  const { loadProfile, loadingProfile, profile, setActiveTab } = profileStore;

  useEffect(() => {
    profileStore.loadHeaders();
    if (username) loadProfile(username);
    return () => {
      setActiveTab(0);
    };
  }, [loadProfile, username, setActiveTab, commentStore, profileStore]);

  if (loadingProfile) return <LoadingComponents content="Loading profile..." />;
  return (
    <>
      <div style={{ width: "100%", textAlign: "center" }}>
        <Header
          style={{ margin: "0 auto" }}
          size="huge"
          color="teal"
          content={"profile of " + profile?.displayName}
        />
        <h4 style={{ margin: "0 aut" }}>
          And You Are {userStore.user?.displayName}
        </h4>

        {/* <Grid.Column width={16}>
        {profile && (
          <>
            <ProfileHeader profile={profile} />
            <ProfileContent profile={profile} />
          </>
        )}
      </Grid.Column> */}
      </div>
      <Divider />
      {userStore.user?.userName === profile?.username && (
        <>
          {profileStore.profileHeaders.map((x) => (
            <Button
              key={x}
              fluid
              positive
              content={"send Messaget to " + x}
              onClick={() => {
                setTalkTo(x);
              }}
            />
          ))}
        </>
      )}
      <Divider />
      <Header style={{ textAlign: "center" }}>
        Now you are talking to {!!TalkTo ? TalkTo : "Nobody"}
      </Header>
      <br />
      <Button
        content="Close Chat"
        onClick={() => {
          setTalkTo(null);
          commentStore.clearPrivateComments();
        }}
      />
      {!!TalkTo && <TestComments username={TalkTo} />}
    </>
  );
});
